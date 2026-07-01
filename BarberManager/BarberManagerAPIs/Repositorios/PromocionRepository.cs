using Microsoft.EntityFrameworkCore;
using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;

namespace BarberManagerAPIs.Repositorios;

public interface IPromocionRepository
{
    Task<List<Promocion>> ObtenerTodos();
    Task<Promocion?> ObtenerPorId(int id);
    Task Agregar(Promocion promocion);
    Task Guardar();
}

public class PromocionRepository : IPromocionRepository
{
    private readonly AppDbContext _context;

    public PromocionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Promocion>> ObtenerTodos()
    {
        return await _context.Promociones.ToListAsync();
    }

    public async Task<Promocion?> ObtenerPorId(int id)
    {
        return await _context.Promociones
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task Agregar(Promocion promocion)
    {
        _context.Promociones.Add(promocion);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }
}