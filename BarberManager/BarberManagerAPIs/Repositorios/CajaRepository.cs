using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BarberManagerAPIs.Repositorios;

public interface ICajaRepository
{
    Task<List<Caja>> ObtenerTodos();
    Task<Caja?> ObtenerPorId(int id);
    Task Agregar(Caja caja);
    Task Guardar();
}

public class CajaRepository : ICajaRepository
{
    private readonly AppDbContext _context;

    public CajaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Caja>> ObtenerTodos()
    {
        return await _context.Cajas.ToListAsync();
    }

    public async Task<Caja?> ObtenerPorId(int id)
    {
        return await _context.Cajas.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task Agregar(Caja caja)
    {
        _context.Cajas.Add(caja);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }
}