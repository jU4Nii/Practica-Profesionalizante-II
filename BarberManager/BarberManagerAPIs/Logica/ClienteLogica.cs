using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IClienteLogica
{
    Task<List<Cliente>> ObtenerTodos();

    Task<Cliente?> ObtenerPorId(int id);

    Task<bool> Agregar(ClienteDTO dto);
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
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return false;

        Cliente cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Correo = dto.Correo
        };

        await _repository.Agregar(cliente);

        return true;
    }
}