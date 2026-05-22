using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Datos;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Repositorios;

public interface IAnimalRepository
{
    Task<List<Animal>> ObtenerTodos();
    Task<Animal?> ObtenerPorId(int id);
    Task Agregar(Animal animal);
    Task Guardar();
    Task Eliminar(Animal animal);
}

public class AnimalRepository : IAnimalRepository
{
    private readonly AppDbContext _context;

    public AnimalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Animal>> ObtenerTodos()
    {
        return await _context.Animales.ToListAsync();
    }

    public async Task<Animal?> ObtenerPorId(int id)
    {
        return await _context.Animales.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task Agregar(Animal animal)
    {
        _context.Animales.Add(animal);
        await _context.SaveChangesAsync();
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Animal animal)
    {
        _context.Animales.Remove(animal);
        await _context.SaveChangesAsync();
    }
}