using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IProductoLogica
{
    Task<List<Producto>> ObtenerTodos();

    Task<Producto?> ObtenerPorId(int id);

    Task<bool> Agregar(ProductoDTO dto);

    Task<bool> Editar(int id, ProductoDTO dto);
    Task<bool> Eliminar(int id);

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

    public async Task<bool> Editar(int id, ProductoDTO dto)
    {
        var producto = await _repository.ObtenerPorId(id);

        if (producto == null)
            return false;

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Cantidad = dto.Cantidad;
        producto.UsoInterno = dto.UsoInterno;
        producto.Precio = dto.Precio;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var producto = await _repository.ObtenerPorId(id);

        if (producto == null)
            return false;

        await _repository.Eliminar(producto);

        return true;
    }

}