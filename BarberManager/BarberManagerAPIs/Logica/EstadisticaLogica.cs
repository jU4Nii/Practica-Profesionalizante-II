using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IEstadisticaLogica
{
    Task<List<Estadistica>> ObtenerTodos();
    Task<Estadistica?> ObtenerPorId(int id);
    Task<bool> Agregar(EstadisticaDTO dto);
    Task RegistrarServicio(DateTime fecha);
    Task RegistrarVenta(DateTime fecha);
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

    public async Task RegistrarServicio(DateTime fecha)
    {
        var estadistica = await ObtenerOCrearPorFecha(fecha);
        estadistica.CantServicios++;
        await _repository.Guardar();
    }

    public async Task RegistrarVenta(DateTime fecha)
    {
        var estadistica = await ObtenerOCrearPorFecha(fecha);
        estadistica.CantVentas++;
        await _repository.Guardar();
    }

    private async Task<Estadistica> ObtenerOCrearPorFecha(DateTime fecha)
    {
        var estadistica = (await _repository.ObtenerTodos())
            .FirstOrDefault(e => e.Fecha.Date == fecha.Date);

        if (estadistica != null)
            return estadistica;

        estadistica = new Estadistica
        {
            Fecha = fecha.Date,
            NombreDia = fecha.ToString("dddd", new System.Globalization.CultureInfo("es-AR")),
            CantServicios = 0,
            CantVentas = 0
        };

        await _repository.Agregar(estadistica);
        return estadistica;
    }
}
