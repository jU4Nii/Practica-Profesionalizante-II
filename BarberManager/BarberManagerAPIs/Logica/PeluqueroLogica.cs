using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface IPeluqueroLogica
{
    Task<List<Peluquero>> ObtenerTodos();
    Task<Peluquero?> ObtenerPorId(int id);
    Task<bool> Agregar(PeluqueroDTO dto);

    Task<bool> Editar(int id, PeluqueroDTO dto);
    Task<bool> Eliminar(int id);

}

public class PeluqueroLogica : IPeluqueroLogica
{
    private readonly IPeluqueroRepository _repository;

    public PeluqueroLogica(IPeluqueroRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Peluquero>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Peluquero?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(PeluqueroDTO dto)
    {
        Peluquero peluquero = new()
        {
            Nombre = dto.Nombre,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Contrasena = dto.Contrasena,
            EsAdmin = dto.EsAdmin,
            EstaActivo = dto.EstaActivo
        };

        await _repository.Agregar(peluquero);

        return true;
    }

    public async Task<bool> Editar(int id, PeluqueroDTO dto)
    {
        var peluquero = await _repository.ObtenerPorId(id);

        if (peluquero == null)
            return false;

        peluquero.Nombre = dto.Nombre;
        peluquero.Correo = dto.Correo;
        peluquero.Telefono = dto.Telefono;
        peluquero.Contrasena = dto.Contrasena;
        peluquero.EsAdmin = dto.EsAdmin;
        peluquero.EstaActivo = dto.EstaActivo;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var peluquero = await _repository.ObtenerPorId(id);

        if (peluquero == null)
            return false;

        await _repository.Eliminar(peluquero);

        return true;
    }

}