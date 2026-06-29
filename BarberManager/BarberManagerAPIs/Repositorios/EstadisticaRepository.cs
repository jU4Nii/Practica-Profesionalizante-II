using Microsoft.EntityFrameworkCore;
using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;

namespace BarberManagerAPIs.Repositorios;

public interface IEstadisticaRepository
{
    Task<List<Estadistica>> ObtenerTodos();
    Task<Estadistica?> ObtenerPorId(int id);
    Task Agregar(Estadistica estadistica);
}

public class EstadisticaRepository : IEstadisticaRepository
{
    private readonly AppDbContext _context;

    public EstadisticaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Estadistica>> ObtenerTodos()
    {
        return await _context.Estadisticas.ToListAsync();
    }

    public async Task<Estadistica?> ObtenerPorId(int id)
    {
        return await _context.Estadisticas
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task Agregar(Estadistica estadistica)
    {
        _context.Estadisticas.Add(estadistica);
        await _context.SaveChangesAsync();
    }
}