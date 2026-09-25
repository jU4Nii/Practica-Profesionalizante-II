using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IClienteLogica
{
    Task<List<Cliente>> ObtenerTodos();

    Task<Cliente?> ObtenerPorId(int id);

    Task<bool> Agregar(ClienteDTO dto);
    Task<bool> Editar(int id, ClienteDTO dto);
}

public class ClienteLogica : IClienteLogica
{
    private readonly IClienteRepository _repository;

    public ClienteLogica(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Cliente>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Cliente?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(ClienteDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre) ||
    string.IsNullOrWhiteSpace(dto.Telefono))
            return false;

        Cliente cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            Notas = dto.Notas
        };

        await _repository.Agregar(cliente);

        return true;
    }

    public async Task<bool> Editar(int id, ClienteDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Telefono))
            return false;

        var cliente = await _repository.ObtenerPorId(id);
        if (cliente == null)
            return false;

        cliente.Nombre = dto.Nombre;
        cliente.Telefono = dto.Telefono;
        cliente.Correo = dto.Correo;
        cliente.Notas = dto.Notas;
        await _repository.Guardar();
        return true;
    }
}
