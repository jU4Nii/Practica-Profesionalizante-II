using VeterinariaAPI.Entidades;
using VeterinariaAPI.Repositorios;

namespace VeterinariaAPI.Logica;

public interface ITratamientoLogica
{
    Task<List<Tratamiento>> ObtenerTodos();
    Task<Tratamiento?> ObtenerPorId(int id);
    Task Agregar(string descripcion);
    Task<bool> Editar(int id, string descripcion);
    Task<bool> Eliminar(int id);
}

public class TratamientoLogica : ITratamientoLogica
{
    private readonly ITratamientoRepository _repo;

    public TratamientoLogica(ITratamientoRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Tratamiento>> ObtenerTodos()
        => _repo.ObtenerTodos();

    public Task<Tratamiento?> ObtenerPorId(int id)
        => _repo.ObtenerPorId(id);

    public async Task Agregar(string descripcion)
    {
        await _repo.Agregar(new Tratamiento { Descripcion = descripcion });
    }

    public async Task<bool> Editar(int id, string descripcion)
    {
        var t = await _repo.ObtenerPorId(id);
        if (t == null) return false;

        t.Descripcion = descripcion;
        await _repo.Guardar();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var t = await _repo.ObtenerPorId(id);
        if (t == null) return false;

        await _repo.Eliminar(t);
        return true;
    }
}