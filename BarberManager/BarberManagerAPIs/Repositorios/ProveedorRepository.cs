using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BarberManagerAPIs.Repositorios;

public interface IProveedorRepository
{
    Task<List<Proveedor>> ObtenerTodos();

    Task<Proveedor?> ObtenerPorId(int id);

    Task Agregar(Proveedor proveedor);

    Task Guardar();
}

public class ProveedorRepository : IProveedorRepository
{
    private readonly AppDbContext _context;

    public ProveedorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Proveedor>> ObtenerTodos()
    {
        return await _context.Proveedores.ToListAsync();
    }

    public async Task<Proveedor?> ObtenerPorId(int id)
    {
        return await _context.Proveedores
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task Agregar(Proveedor proveedor)
    {
        _context.Proveedores.Add(proveedor);

        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }
}
