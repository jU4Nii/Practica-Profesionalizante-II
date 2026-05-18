using VeterinariaAPI.Entidades;
using VeterinariaAPI.Repositorios;

namespace VeterinariaAPI.Logica;

public interface IMedicamentoLogica
{
    Task<List<Medicamento>> ObtenerTodos();
    Task<Medicamento?> ObtenerPorId(int id);
    Task Agregar(string nombre);
    Task<bool> Editar(int id, string nombre);
    Task<bool> Eliminar(int id);
}

public class MedicamentoLogica : IMedicamentoLogica
{
    private readonly IMedicamentoRepository _repo;

    public MedicamentoLogica(IMedicamentoRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Medicamento>> ObtenerTodos()
        => _repo.ObtenerTodos();

    public Task<Medicamento?> ObtenerPorId(int id)
        => _repo.ObtenerPorId(id);

    public async Task Agregar(string nombre)
    {
        await _repo.Agregar(new Medicamento { Nombre = nombre });
    }

    public async Task<bool> Editar(int id, string nombre)
    {
        var m = await _repo.ObtenerPorId(id);
        if (m == null) return false;

        m.Nombre = nombre;
        await _repo.Guardar();
        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var m = await _repo.ObtenerPorId(id);
        if (m == null) return false;

        await _repo.Eliminar(m);
        return true;
    }
}