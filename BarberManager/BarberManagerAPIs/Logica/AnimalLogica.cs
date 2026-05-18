using VeterinariaAPI.Entidades;
using VeterinariaAPI.Logica.DTOs;
using VeterinariaAPI.Repositorios;

namespace VeterinariaAPI.Logica;

public interface IAnimalLogica
{
    Task<List<Animal>> ObtenerTodos();
    Task<Animal?> ObtenerPorId(int id);
    Task Agregar(AnimalDTO dto);
    Task<bool> Editar(int id, AnimalDTO dto);
    Task<bool> Eliminar(int id);
}

public class AnimalLogica : IAnimalLogica
{
    private readonly IAnimalRepository _repository;

    public AnimalLogica(IAnimalRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Animal>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Animal?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task Agregar(AnimalDTO dto)
    {
        Animal animal = new Animal
        {
            Nombre = dto.Nombre,
            Edad = dto.Edad,
            Sexo = dto.Sexo,
            RazaId = dto.RazaId,
            DuenoId = dto.DuenoId
        };

        await _repository.Agregar(animal);
    }

    public async Task<bool> Editar(int id, AnimalDTO dto)
    {
        var animal = await _repository.ObtenerPorId(id);

        if (animal == null)
            return false;

        animal.Nombre = dto.Nombre;
        animal.Edad = dto.Edad;
        animal.Sexo = dto.Sexo;
        animal.RazaId = dto.RazaId;
        animal.DuenoId = dto.DuenoId;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var animal = await _repository.ObtenerPorId(id);

        if (animal == null)
            return false;

        await _repository.Eliminar(animal);

        return true;
    }
}