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
    Task<bool> Agregar(TurnoDTO dto);
    Task<bool> Editar(int id, TurnoDTO dto);
    Task<bool> Eliminar(int id);
}

public class TurnoLogica : ITurnoLogica
{
    private readonly ITurnoRepository _repository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPeluqueroRepository _peluqueroRepository;

    public TurnoLogica(
        ITurnoRepository repository,
        IClienteRepository clienteRepository,
        IPeluqueroRepository peluqueroRepository)
    {
        _repository = repository;
        _clienteRepository = clienteRepository;
        _peluqueroRepository = peluqueroRepository;
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

    public async Task<bool> Agregar(TurnoDTO dto)
    {
        var cliente = await _clienteRepository.ObtenerPorId(dto.IdCliente);

        if (cliente == null)
            return false;

        var peluquero = await _peluqueroRepository.ObtenerPorId(dto.IdPeluquero);

        if (peluquero == null)
            return false;

        var turnos = await _repository.ObtenerTodos();

        bool ocupado = turnos.Any(t =>
            t.IdPeluquero == dto.IdPeluquero &&
            t.Fecha.Date == dto.Fecha.Date &&
            t.Hora == dto.Hora &&
            !t.Cancelado);

        if (ocupado)
            return false;

        Turno turno = new Turno
        {
            IdCliente = dto.IdCliente,
            IdPeluquero = dto.IdPeluquero,
            Fecha = dto.Fecha,
            Hora = dto.Hora,
            IdPromocion = dto.IdPromocion
        };

        await _repository.Agregar(turno);

        return true;
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

        return true;
    }
}