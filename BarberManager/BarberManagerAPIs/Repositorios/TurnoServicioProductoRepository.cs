using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BarberManagerAPIs.Repositorios;

public interface ITurnoServicioProductoRepository
{
    Task<List<TurnoServicioProducto>> ObtenerTodos();
    Task<TurnoServicioProducto?> ObtenerPorId(int id);
    Task Agregar(TurnoServicioProducto item);
    Task EliminarPorTurno(int idTurno);
}

public class TurnoServicioProductoRepository : ITurnoServicioProductoRepository
{
    private readonly AppDbContext _context;

    public TurnoServicioProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TurnoServicioProducto>> ObtenerTodos()
    {
        return await _context.TurnoServicioProductos.ToListAsync();
    }

    public async Task<TurnoServicioProducto?> ObtenerPorId(int id)
    {
        return await _context.TurnoServicioProductos.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Agregar(TurnoServicioProducto item)
    {
        _context.TurnoServicioProductos.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarPorTurno(int idTurno)
    {
        var items = await _context.TurnoServicioProductos
            .Where(x => x.IdTurno == idTurno)
            .ToListAsync();

        _context.TurnoServicioProductos.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
}
