using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IServicioLogica
{
    Task<List<Servicio>> ObtenerTodos();

    Task<Servicio?> ObtenerPorId(int id);

    Task<bool> Agregar(ServicioDTO dto);
}

public class ServicioLogica : IServicioLogica
{
    private readonly IServicioRepository _repository;

    public ServicioLogica(IServicioRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Servicio>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Servicio?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(ServicioDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return false;

        if (dto.Precio <= 0)
            return false;

        Servicio servicio = new Servicio
        {
            Nombre = dto.Nombre,
            Precio = dto.Precio
        };

        await _repository.Agregar(servicio);

        return true;
    }
}