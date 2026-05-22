using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Datos;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Repositorios;

public interface IMedicamentoRepository
{
    Task<List<Medicamento>> ObtenerTodos();
    Task<Medicamento?> ObtenerPorId(int id);
    Task Agregar(Medicamento m);
    Task Guardar();
    Task Eliminar(Medicamento m);
}

public class MedicamentoRepository : IMedicamentoRepository
{
    private readonly AppDbContext _context;

    public MedicamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Medicamento>> ObtenerTodos()
        => _context.Medicamentos.ToListAsync();

    public Task<Medicamento?> ObtenerPorId(int id)
        => _context.Medicamentos.FirstOrDefaultAsync(x => x.Id == id);

    public async Task Agregar(Medicamento m)
    {
        _context.Medicamentos.Add(m);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
        => await _context.SaveChangesAsync();

    public async Task Eliminar(Medicamento m)
    {
        _context.Medicamentos.Remove(m);
        await _context.SaveChangesAsync();
    }
}