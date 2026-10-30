using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IProveedorLogica
{
    Task<List<Proveedor>> ObtenerTodos();

    Task<Proveedor?> ObtenerPorId(int id);

    Task<Proveedor?> Agregar(ProveedorDTO dto);

    Task<bool> Editar(int id, ProveedorDTO dto);
}

public class ProveedorLogica : IProveedorLogica
{
    private readonly IProveedorRepository _repository;

    public ProveedorLogica(IProveedorRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Proveedor>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Proveedor?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<Proveedor?> Agregar(ProveedorDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre) ||
            string.IsNullOrWhiteSpace(dto.Telefono) ||
            string.IsNullOrWhiteSpace(dto.Email))
            return null;

        Proveedor proveedor = new Proveedor
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Email = dto.Email
        };

        await _repository.Agregar(proveedor);

        return proveedor;
    }

    public async Task<bool> Editar(int id, ProveedorDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre) ||
            string.IsNullOrWhiteSpace(dto.Telefono) ||
            string.IsNullOrWhiteSpace(dto.Email))
            return false;

        var proveedor = await _repository.ObtenerPorId(id);
        if (proveedor == null)
            return false;

        proveedor.Nombre = dto.Nombre;
        proveedor.Telefono = dto.Telefono;
        proveedor.Email = dto.Email;
        await _repository.Guardar();
        return true;
    }
}
