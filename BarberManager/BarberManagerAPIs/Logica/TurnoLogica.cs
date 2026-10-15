using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface ITurnoLogica
{
    Task<List<Turno>> ObtenerTodos();
    Task<Turno?> ObtenerPorId(int id);
    Task<List<Turno>> ObtenerPorFecha(DateTime fecha);
    Task<List<Turno>> ObtenerPorPeluquero(int idPeluquero);
    Task<Turno?> Agregar(TurnoDTO dto);
    Task<bool> Editar(int id, TurnoDTO dto);
    Task<bool> Eliminar(int id);
    Task<bool> EliminarDefinitivamente(int id);
}

public class TurnoLogica : ITurnoLogica
{
    private readonly ITurnoRepository _repository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPeluqueroRepository _peluqueroRepository;
    private readonly IEstadisticaLogica _estadisticaLogica;
    private readonly ITurnoServicioProductoRepository _turnoServicioProductoRepository;
    private readonly ICajaRepository _cajaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IServicioRepository _servicioRepository;

    public TurnoLogica(
        ITurnoRepository repository,
        IClienteRepository clienteRepository,
        IPeluqueroRepository peluqueroRepository,
        IEstadisticaLogica estadisticaLogica,
        ITurnoServicioProductoRepository turnoServicioProductoRepository,
        ICajaRepository cajaRepository,
        IProductoRepository productoRepository,
        IServicioRepository servicioRepository)
    {
        _repository = repository;
        _clienteRepository = clienteRepository;
        _peluqueroRepository = peluqueroRepository;
        _estadisticaLogica = estadisticaLogica;
        _turnoServicioProductoRepository = turnoServicioProductoRepository;
        _cajaRepository = cajaRepository;
        _productoRepository = productoRepository;
        _servicioRepository = servicioRepository;
    }

    public async Task<List<Turno>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Turno?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<List<Turno>> ObtenerPorFecha(DateTime fecha)
    {
        return await _repository.ObtenerPorFecha(fecha);
    }

    public async Task<List<Turno>> ObtenerPorPeluquero(int idPeluquero)
    {
        return await _repository.ObtenerPorPeluquero(idPeluquero);
    }

    public async Task<Turno?> Agregar(TurnoDTO dto)
    {
        var cliente = await _clienteRepository.ObtenerPorId(dto.IdCliente);

        if (cliente == null)
            return null;

        var peluquero = await _peluqueroRepository.ObtenerPorId(dto.IdPeluquero);

        if (peluquero == null)
            return null;

        var turnos = await _repository.ObtenerTodos();

        bool ocupado = turnos.Any(t =>
            t.IdPeluquero == dto.IdPeluquero &&
            t.Fecha.Date == dto.Fecha.Date &&
            t.Hora == dto.Hora &&
            !t.Cancelado);

        if (ocupado)
            return null;

        Turno turno = new Turno
        {
            IdCliente = dto.IdCliente,
            IdPeluquero = dto.IdPeluquero,
            Fecha = dto.Fecha,
            Hora = dto.Hora,
            IdPromocion = dto.IdPromocion
        };

        await _repository.Agregar(turno);
        await _estadisticaLogica.RegistrarServicio(dto.Fecha);

        return turno;
    }

    public async Task<bool> Editar(int id, TurnoDTO dto)
    {
        var turno = await _repository.ObtenerPorId(id);

        if (turno == null)
            return false;

        var cliente = await _clienteRepository.ObtenerPorId(dto.IdCliente);

        if (cliente == null)
            return false;

        var peluquero = await _peluqueroRepository.ObtenerPorId(dto.IdPeluquero);

        if (peluquero == null)
            return false;

        var turnos = await _repository.ObtenerTodos();

        bool ocupado = turnos.Any(t =>
            t.Id != id &&
            t.IdPeluquero == dto.IdPeluquero &&
            t.Fecha.Date == dto.Fecha.Date &&
            t.Hora == dto.Hora &&
            !t.Cancelado);

        if (ocupado)
            return false;

        turno.IdCliente = dto.IdCliente;
        turno.IdPeluquero = dto.IdPeluquero;
        turno.Fecha = dto.Fecha;
        turno.Hora = dto.Hora;
        turno.IdPromocion = dto.IdPromocion;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var turno = await _repository.ObtenerPorId(id);

        if (turno == null)
            return false;

        turno.Cancelado = true;
        await _repository.Guardar();

        try
        {
            // 1. Descontar el servicio de las estadísticas
            await _estadisticaLogica.DescontarServicio(turno.Fecha);

            // 2. Obtener los items asociados a este turno
            var items = await _turnoServicioProductoRepository.ObtenerTodos();
            var itemsTurno = items.Where(i => i.IdTurno == id).ToList();

            // 3. Restaurar stock de los productos asociados al turno
            foreach (var item in itemsTurno.Where(i => i.IdProducto.HasValue && i.CantidadProducto > 0))
            {
                var prod = await _productoRepository.ObtenerPorId(item.IdProducto!.Value);
                if (prod != null)
                {
                    prod.Cantidad += item.CantidadProducto;
                    await _productoRepository.Guardar();
                }
            }

            // 4. Restar / eliminar de Caja los ingresos generados por este turno
            var tag = $"(Turno #{id})";
            var movimientosCaja = await _cajaRepository.ObtenerTodos();
            var cajasTurno = movimientosCaja
                .Where(c => c.EsIngreso && c.Concepto != null && c.Concepto.Contains(tag))
                .ToList();

            if (!cajasTurno.Any())
            {
                foreach (var item in itemsTurno.Where(i => i.IdServicio.HasValue))
                {
                    var serv = await _servicioRepository.ObtenerPorId(item.IdServicio!.Value);
                    if (serv != null)
                    {
                        var match = movimientosCaja.FirstOrDefault(c =>
                            c.EsIngreso &&
                            c.Concepto == $"Servicio: {serv.Nombre}" &&
                            c.Monto == serv.Precio &&
                            c.Fecha.Date == turno.Fecha.Date);
                        if (match != null && !cajasTurno.Contains(match))
                        {
                            cajasTurno.Add(match);
                        }
                    }
                }
            }

            foreach (var c in cajasTurno)
            {
                await _cajaRepository.Eliminar(c);
            }
        }
        catch
        {
            // La cancelación del turno ya quedó registrada
        }

        return true;
    }

    public async Task<bool> EliminarDefinitivamente(int id)
    {
        var turno = await _repository.ObtenerPorId(id);

        if (turno == null || !turno.Cancelado)
            return false;

        await _turnoServicioProductoRepository.EliminarPorTurno(id);
        await _repository.Eliminar(turno);

        return true;
    }
}
