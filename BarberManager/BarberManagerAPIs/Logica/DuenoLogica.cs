using VeterinariaAPI.Entidades;
using VeterinariaAPI.Logica.DTOs;
using VeterinariaAPI.Repositorios;

namespace VeterinariaAPI.Logica;

public interface IDuenoLogica
{
    Task<List<Dueno>> ObtenerTodos();
    Task<Dueno?> ObtenerPorId(int id);
    Task Agregar(DuenoDTO dto);
    Task<bool> Editar(int id, DuenoDTO dto);
    Task<bool> Eliminar(int id);
}

public class DuenoLogica : IDuenoLogica
{
    private readonly IDuenoRepository _repository;

    public DuenoLogica(IDuenoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Dueno>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Dueno?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task Agregar(DuenoDTO dto)
    {
        Dueno dueno = new Dueno
        {
            Dni = dto.Dni,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido
        };

        await _repository.Agregar(dueno);
    }

    public async Task<bool> Editar(int id, DuenoDTO dto)
    {
        var dueno = await _repository.ObtenerPorId(id);

        if (dueno == null)
            return false;

        dueno.Dni = dto.Dni;
        dueno.Nombre = dto.Nombre;
        dueno.Apellido = dto.Apellido;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var dueno = await _repository.ObtenerPorId(id);

        if (dueno == null)
            return false;

        await _repository.Eliminar(dueno);

        return true;
    }
}