using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface ITurnoServicioProductoLogica
{
    Task<List<TurnoServicioProducto>> ObtenerTodos();
    Task<TurnoServicioProducto?> ObtenerPorId(int id);
    Task Agregar(TurnoServicioProductoDTO dto);
}

public class TurnoServicioProductoLogica : ITurnoServicioProductoLogica
{
    private readonly ITurnoServicioProductoRepository _repository;

    public TurnoServicioProductoLogica(ITurnoServicioProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TurnoServicioProducto>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<TurnoServicioProducto?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task Agregar(TurnoServicioProductoDTO dto)
    {
        TurnoServicioProducto item = new TurnoServicioProducto
        {
            IdTurno = dto.IdTurno,
            IdServicio = dto.IdServicio,
            IdProducto = dto.IdProducto,
            CantidadProducto = dto.CantidadProducto,
            PrecioUnitario = dto.PrecioUnitario
        };

        await _repository.Agregar(item);
    }
}