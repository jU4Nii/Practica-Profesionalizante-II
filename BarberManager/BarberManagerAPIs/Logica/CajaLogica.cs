using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;
using BarberManagerAPIs.Datos;

namespace BarberManagerAPIs.Logica;

public interface ICajaLogica
{
    Task<List<Caja>> ObtenerTodos();
    Task<Caja?> ObtenerPorId(int id);
    Task Agregar(CajaDTO dto);
    Task<bool> Editar(int id, CajaDTO dto);
    Task<bool> RegistrarVentaProducto(VentaProductoDTO dto);
}

public class CajaLogica : ICajaLogica
{
    private readonly ICajaRepository _repository;
    private readonly IEstadisticaLogica _estadisticaLogica;
    private readonly IProductoRepository _productoRepository;
    private readonly AppDbContext _context;

    public CajaLogica(
        ICajaRepository repository,
        IEstadisticaLogica estadisticaLogica,
        IProductoRepository productoRepository,
        AppDbContext context)
    {
        _repository = repository;
        _estadisticaLogica = estadisticaLogica;
        _productoRepository = productoRepository;
        _context = context;
    }

    public async Task<List<Caja>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Caja?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task Agregar(CajaDTO dto)
    {
        Caja caja = new Caja
        {
            Fecha = dto.Fecha,
            Monto = dto.Monto,
            Concepto = dto.Concepto,
            MetodoPago = dto.MetodoPago,
            EsIngreso = dto.EsIngreso
        };

        await _repository.Agregar(caja);

        if (dto.EsIngreso && dto.Concepto == "Venta de producto")
            await _estadisticaLogica.RegistrarVenta(dto.Fecha);
    }

    public async Task<bool> Editar(int id, CajaDTO dto)
    {
        var caja = await _repository.ObtenerPorId(id);

        if (caja == null)
            return false;

        caja.Fecha = dto.Fecha;
        caja.Monto = dto.Monto;
        caja.Concepto = dto.Concepto;
        caja.MetodoPago = dto.MetodoPago;
        caja.EsIngreso = dto.EsIngreso;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> RegistrarVentaProducto(VentaProductoDTO dto)
    {
        if (dto.Cantidad <= 0 || string.IsNullOrWhiteSpace(dto.MetodoPago))
            return false;

        var producto = await _productoRepository.ObtenerPorId(dto.IdProducto);
        if (producto == null || producto.UsoInterno || producto.Cantidad < dto.Cantidad)
            return false;

        await using var transaccion = await _context.Database.BeginTransactionAsync();
        try
        {
            producto.Cantidad -= dto.Cantidad;
            await _productoRepository.Guardar();
            await _repository.Agregar(new Caja
            {
                Fecha = dto.Fecha.Date,
                Monto = producto.Precio * dto.Cantidad,
                Concepto = $"Venta de producto: {producto.Nombre}",
                MetodoPago = dto.MetodoPago,
                EsIngreso = true
            });
            await _estadisticaLogica.RegistrarVenta(dto.Fecha);
            await transaccion.CommitAsync();
            return true;
        }
        catch
        {
            await transaccion.RollbackAsync();
            return false;
        }
    }
}
