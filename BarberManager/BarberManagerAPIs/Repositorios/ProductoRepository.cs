using Microsoft.EntityFrameworkCore;
using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;

namespace BarberManagerAPIs.Repositorios;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerTodos();

    Task<Producto?> ObtenerPorId(int id);

    Task Agregar(Producto producto);

    Task Guardar();
    Task Eliminar(Producto producto);

}

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> ObtenerTodos()
    {
        return await _context.Productos.ToListAsync();
    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task Agregar(Producto producto)
    {
        _context.Productos.Add(producto);

        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Producto producto)
    {
        _context.Productos.Remove(producto);

        await _context.SaveChangesAsync();
    }

}