using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BarberManagerAPIs.Repositorios;

public interface ITurnoRepository
{
    Task<List<Turno>> ObtenerTodos();
    Task<Turno?> ObtenerPorId(int id);
    Task<List<Turno>> ObtenerPorFecha(DateTime fecha);
    Task<List<Turno>> ObtenerPorPeluquero(int idPeluquero);
    Task Agregar(Turno turno);
    Task Guardar();
    Task Eliminar(Turno turno);
}

public class TurnoRepository : ITurnoRepository
{
    private readonly AppDbContext _context;

    public TurnoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Turno>> ObtenerTodos()
    {
        return await _context.Turnos.ToListAsync();
    }

    public async Task<Turno?> ObtenerPorId(int id)
    {
        return await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Turno>> ObtenerPorFecha(DateTime fecha)
    {
        return await _context.Turnos
            .Where(t => t.Fecha.Date == fecha.Date)
            .ToListAsync();
    }

    public async Task<List<Turno>> ObtenerPorPeluquero(int idPeluquero)
    {
        return await _context.Turnos
            .Where(t => t.IdPeluquero == idPeluquero)
            .ToListAsync();
    }

    public async Task Agregar(Turno turno)
    {
        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Turno turno)
    {
        _context.Turnos.Remove(turno);
        await _context.SaveChangesAsync();
    }
}