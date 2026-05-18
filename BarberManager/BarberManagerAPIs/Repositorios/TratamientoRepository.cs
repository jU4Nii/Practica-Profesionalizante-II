using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Datos;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Repositorios;

public interface ITratamientoRepository
{
    Task<List<Tratamiento>> ObtenerTodos();
    Task<Tratamiento?> ObtenerPorId(int id);
    Task Agregar(Tratamiento t);
    Task Guardar();
    Task Eliminar(Tratamiento t);
}

public class TratamientoRepository : ITratamientoRepository
{
    private readonly AppDbContext _context;

    public TratamientoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Tratamiento>> ObtenerTodos()
        => _context.Tratamientos.ToListAsync();

    public Task<Tratamiento?> ObtenerPorId(int id)
        => _context.Tratamientos.FirstOrDefaultAsync(x => x.Id == id);

    public async Task Agregar(Tratamiento t)
    {
        _context.Tratamientos.Add(t);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
        => await _context.SaveChangesAsync();

    public async Task Eliminar(Tratamiento t)
    {
        _context.Tratamientos.Remove(t);
        await _context.SaveChangesAsync();
    }
}