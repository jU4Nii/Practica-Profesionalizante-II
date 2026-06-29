using Microsoft.EntityFrameworkCore;
using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;

namespace BarberManagerAPIs.Repositorios;

public interface IPeluqueroRepository
{
    Task<List<Peluquero>> ObtenerTodos();
    Task<Peluquero?> ObtenerPorId(int id);
    Task Agregar(Peluquero peluquero);
}

public class PeluqueroRepository : IPeluqueroRepository
{
    private readonly AppDbContext _context;

    public PeluqueroRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Peluquero>> ObtenerTodos()
    {
        return await _context.Peluqueros.ToListAsync();
    }

    public async Task<Peluquero?> ObtenerPorId(int id)
    {
        return await _context.Peluqueros
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Agregar(Peluquero peluquero)
    {
        _context.Peluqueros.Add(peluquero);
        await _context.SaveChangesAsync();
    }
}