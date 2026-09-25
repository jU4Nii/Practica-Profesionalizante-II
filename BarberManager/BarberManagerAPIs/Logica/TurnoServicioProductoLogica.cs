using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface ITurnoServicioProductoLogica
{
    Task<List<TurnoServicioProducto>> ObtenerTodos();
    Task<TurnoServicioProducto?> ObtenerPorId(int id);
    Task<bool> Agregar(TurnoServicioProductoDTO dto);
}

public class TurnoServicioProductoLogica : ITurnoServicioProductoLogica
{
    private readonly ITurnoServicioProductoRepository _repository;
    private readonly IProductoRepository _productoRepository;
    private readonly IServicioRepository _servicioRepository;
    private readonly ICajaLogica _cajaLogica;
    private readonly AppDbContext _context;

    public TurnoServicioProductoLogica(
        ITurnoServicioProductoRepository repository,
        IProductoRepository productoRepository,
        IServicioRepository servicioRepository,
        ICajaLogica cajaLogica,
        AppDbContext context)
    {
        _repository = repository;
        _productoRepository = productoRepository;
        _servicioRepository = servicioRepository;
        _cajaLogica = cajaLogica;
        _context = context;
    }

    public async Task<List<TurnoServicioProducto>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<TurnoServicioProducto?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(TurnoServicioProductoDTO dto)
    {
        if (dto.IdProducto.HasValue)
        {
            if (dto.CantidadProducto <= 0)
                return false;

            var producto = await _productoRepository.ObtenerPorId(dto.IdProducto.Value);
            if (producto == null || producto.Cantidad < dto.CantidadProducto)
                return false;

            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                producto.Cantidad -= dto.CantidadProducto;
                await _repository.Agregar(new TurnoServicioProducto
                {
                    IdTurno = dto.IdTurno,
                    IdServicio = dto.IdServicio,
                    IdProducto = dto.IdProducto,
                    CantidadProducto = dto.CantidadProducto,
                    PrecioUnitario = dto.PrecioUnitario
                });
                await _productoRepository.Guardar();
                await transaccion.CommitAsync();
                return true;
            }
            catch
            {
                await transaccion.RollbackAsync();
                return false;
            }
        }

        if (!dto.IdServicio.HasValue)
            return false;

        var servicio = await _servicioRepository.ObtenerPorId(dto.IdServicio.Value);
        if (servicio == null)
            return false;

        await using var transaccionServicio = await _context.Database.BeginTransactionAsync();
        try
        {
            await _repository.Agregar(new TurnoServicioProducto
            {
                IdTurno = dto.IdTurno,
                IdServicio = dto.IdServicio,
                IdProducto = dto.IdProducto,
                CantidadProducto = dto.CantidadProducto,
                PrecioUnitario = dto.PrecioUnitario
            });
            await _cajaLogica.Agregar(new CajaDTO
            {
                Fecha = DateTime.Today,
                Monto = servicio.Precio,
                Concepto = $"Servicio: {servicio.Nombre}",
                MetodoPago = "Pendiente",
                EsIngreso = true
            });
            await transaccionServicio.CommitAsync();
            return true;
        }
        catch
        {
            await transaccionServicio.RollbackAsync();
            return false;
        }
    }
}
