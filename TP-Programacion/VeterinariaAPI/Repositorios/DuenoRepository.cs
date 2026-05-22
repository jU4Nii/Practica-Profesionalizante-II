using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Datos;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Repositorios;

public interface IDuenoRepository
{
    Task<List<Dueno>> ObtenerTodos();
    Task<Dueno?> ObtenerPorId(int id);
    Task Agregar(Dueno dueno);
    Task Guardar();
    Task Eliminar(Dueno dueno);
}

public class DuenoRepository : IDuenoRepository
{
    private readonly AppDbContext _context;

    public DuenoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Dueno>> ObtenerTodos()
    {
        return await _context.Duenos.ToListAsync();
    }

    public async Task<Dueno?> ObtenerPorId(int id)
    {
        return await _context.Duenos.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task Agregar(Dueno dueno)
    {
        _context.Duenos.Add(dueno);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Dueno dueno)
    {
        _context.Duenos.Remove(dueno);
        await _context.SaveChangesAsync();
    }
}
