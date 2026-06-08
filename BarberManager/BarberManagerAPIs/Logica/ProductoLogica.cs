using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IProductoLogica
{
    Task<List<Producto>> ObtenerTodos();

    Task<Producto?> ObtenerPorId(int id);

    Task<bool> Agregar(ProductoDTO dto);
}

public class ProductoLogica : IProductoLogica
{
    private readonly IProductoRepository _repository;

    public ProductoLogica(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Producto>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(ProductoDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return false;

        if (dto.Cantidad < 0)
            return false;

        if (dto.Precio <= 0)
            return false;

        Producto producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Cantidad = dto.Cantidad,
            UsoInterno = dto.UsoInterno,
            Precio = dto.Precio
        };

        await _repository.Agregar(producto);

        return true;
    }
}