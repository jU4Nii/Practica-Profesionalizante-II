using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IEstadisticaLogica
{
    Task<List<Estadistica>> ObtenerTodos();
    Task<Estadistica?> ObtenerPorId(int id);
    Task<bool> Agregar(EstadisticaDTO dto);
}

public class EstadisticaLogica : IEstadisticaLogica
{
    private readonly IEstadisticaRepository _repository;

    public EstadisticaLogica(IEstadisticaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Estadistica>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Estadistica?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(EstadisticaDTO dto)
    {
        Estadistica estadistica = new()
        {
            NombreDia = dto.NombreDia,
            Fecha = dto.Fecha,
            CantServicios = dto.CantServicios,
            CantVentas = dto.CantVentas
        };

        await _repository.Agregar(estadistica);

        return true;
    }
}