using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IPromocionLogica
{
    Task<List<Promocion>> ObtenerTodos();
    Task<Promocion?> ObtenerPorId(int id);
    Task Agregar(PromocionDTO dto);
    Task<bool> AsignarServicio(int idPromocion, int idServicio);
    Task<bool> AsignarProducto(int idPromocion, int idProducto);
    Task<bool> EliminarServicio(int idPromocion, int idServicio);
    Task<bool> EliminarProducto(int idPromocion, int idProducto);
}

public class PromocionLogica : IPromocionLogica
{
    private readonly IPromocionRepository _repository;

    public PromocionLogica(IPromocionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Promocion>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Promocion?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task Agregar(PromocionDTO dto)
    {
        Promocion promocion = new()
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            DescuentoPorcentaje = dto.DescuentoPorcentaje,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin
        };

        await _repository.Agregar(promocion);
    }

    public async Task<bool> AsignarServicio(int idPromocion, int idServicio)
    {
        var promocion = await _repository.ObtenerPorId(idPromocion);

        if (promocion == null)
            return false;

        promocion.ServiciosIds.Add(idServicio);

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> AsignarProducto(int idPromocion, int idProducto)
    {
        var promocion = await _repository.ObtenerPorId(idPromocion);

        if (promocion == null)
            return false;

        promocion.ProductosIds.Add(idProducto);

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> EliminarServicio(int idPromocion, int idServicio)
    {
        var promocion = await _repository.ObtenerPorId(idPromocion);

        if (promocion == null)
            return false;

        promocion.ServiciosIds.Remove(idServicio);

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> EliminarProducto(int idPromocion, int idProducto)
    {
        var promocion = await _repository.ObtenerPorId(idPromocion);

        if (promocion == null)
            return false;

        promocion.ProductosIds.Remove(idProducto);

        await _repository.Guardar();

        return true;
    }
}