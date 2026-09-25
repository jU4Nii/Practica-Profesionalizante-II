using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BarberManagerAPIs.Repositorios;

public interface IClienteRepository
{
    Task<List<Cliente>> ObtenerTodos();

    Task<Cliente?> ObtenerPorId(int id);

    Task Agregar(Cliente cliente);
    Task Guardar();
}

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> ObtenerTodos()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorId(int id)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task Agregar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);

        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }
}
