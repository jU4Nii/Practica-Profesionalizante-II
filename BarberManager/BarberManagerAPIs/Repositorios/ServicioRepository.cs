using Microsoft.EntityFrameworkCore;
using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;

namespace BarberManagerAPIs.Repositorios;

public interface IServicioRepository
{
    Task<List<Servicio>> ObtenerTodos();

    Task<Servicio?> ObtenerPorId(int id);

    Task Agregar(Servicio servicio);

    Task Guardar();
    Task Eliminar(Servicio servicio);
}

public class ServicioRepository : IServicioRepository
{
    private readonly AppDbContext _context;

    public ServicioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Servicio>> ObtenerTodos()
    {
        return await _context.Servicios.ToListAsync();
    }

    public async Task<Servicio?> ObtenerPorId(int id)
    {
        return await _context.Servicios
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task Agregar(Servicio servicio)
    {
        _context.Servicios.Add(servicio);

        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Servicio servicio)
    {
        _context.Servicios.Remove(servicio);
        await _context.SaveChangesAsync();
    }

}