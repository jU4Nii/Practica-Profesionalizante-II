using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Datos;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Repositorios;

public interface IRazaRepository
{
    Task<List<Raza>> ObtenerTodos();
    Task<Raza?> ObtenerPorId(int id);
    Task Agregar(Raza raza);
    Task Guardar();
    Task Eliminar(Raza raza);
}

public class RazaRepository : IRazaRepository
{
    private readonly AppDbContext _context;

    public RazaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Raza>> ObtenerTodos()
        => await _context.Razas.ToListAsync();

    public async Task<Raza?> ObtenerPorId(int id)
        => await _context.Razas.FirstOrDefaultAsync(r => r.Id == id);

    public async Task Agregar(Raza raza)
    {
        _context.Razas.Add(raza);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
        => await _context.SaveChangesAsync();

    public async Task Eliminar(Raza raza)
    {
        _context.Razas.Remove(raza);
        await _context.SaveChangesAsync();
    }
}