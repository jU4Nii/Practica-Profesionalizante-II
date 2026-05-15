using VeterinariaAPI.Entidades;
using VeterinariaAPI.Repositorios;

namespace VeterinariaAPI.Logica;

public interface IRazaLogica
{
    Task<List<Raza>> ObtenerTodos();
    Task<Raza?> ObtenerPorId(int id);
    Task Agregar(string nombre);
    Task<bool> Editar(int id, string nombre);
    Task<bool> Eliminar(int id);
}

public class RazaLogica : IRazaLogica
{
    private readonly IRazaRepository _repo;

    public RazaLogica(IRazaRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Raza>> ObtenerTodos() => _repo.ObtenerTodos();

    public Task<Raza?> ObtenerPorId(int id) => _repo.ObtenerPorId(id);

    public async Task Agregar(string nombre)
    {
        await _repo.Agregar(new Raza { Nombre = nombre });
    }

    public async Task<bool> Editar(int id, string nombre)
    {
        var r = await _repo.ObtenerPorId(id);
        if (r == null) return false;

        r.Nombre = nombre;
        await _repo.Guardar();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var r = await _repo.ObtenerPorId(id);
        if (r == null) return false;

        await _repo.Eliminar(r);
        return true;
    }
}