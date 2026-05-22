using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Datos;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Repositorios;

public interface IAtencionRepository
{
    Task<List<Atencion>> ObtenerTodos();
    Task<Atencion?> ObtenerPorId(int id);
    Task Agregar(Atencion atencion);
    Task Guardar();
    Task Eliminar(Atencion atencion);
}

public class AtencionRepository : IAtencionRepository
{
    private readonly AppDbContext _context;

    public AtencionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Atencion>> ObtenerTodos()
    {
        return await _context.Atenciones
            .OrderByDescending(a => a.Fecha)
            .ToListAsync();
    }

    public async Task<Atencion?> ObtenerPorId(int id)
    {
        return await _context.Atenciones
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task Agregar(Atencion atencion)
    {
        _context.Atenciones.Add(atencion);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Atencion atencion)
    {
        _context.Atenciones.Remove(atencion);
        await _context.SaveChangesAsync();
    }
}