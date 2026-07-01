using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface ITurnoLogica
{
    Task<List<Turno>> ObtenerTodos();
    Task<Turno?> ObtenerPorId(int id);
    Task<List<Turno>> ObtenerPorFecha(DateTime fecha);
    Task<List<Turno>> ObtenerPorPeluquero(int idPeluquero);
    Task Agregar(TurnoDTO dto);
    Task<bool> Editar(int id, TurnoDTO dto);
    Task<bool> Eliminar(int id);
}

public class TurnoLogica : ITurnoLogica
{
    private readonly ITurnoRepository _repository;

    public TurnoLogica(ITurnoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Turno>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Turno?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<List<Turno>> ObtenerPorFecha(DateTime fecha)
    {
        return await _repository.ObtenerPorFecha(fecha);
    }

    public async Task<List<Turno>> ObtenerPorPeluquero(int idPeluquero)
    {
        return await _repository.ObtenerPorPeluquero(idPeluquero);
    }

    public async Task Agregar(TurnoDTO dto)
    {
        Turno turno = new Turno
        {
            IdCliente = dto.IdCliente,
            IdPeluquero = dto.IdPeluquero,
            Fecha = dto.Fecha,
            Hora = dto.Hora,
            IdPromocion = dto.IdPromocion
        };

        await _repository.Agregar(turno);
    }

    public async Task<bool> Editar(int id, TurnoDTO dto)
    {
        var turno = await _repository.ObtenerPorId(id);

        if (turno == null)
            return false;

        turno.IdCliente = dto.IdCliente;
        turno.IdPeluquero = dto.IdPeluquero;
        turno.Fecha = dto.Fecha;
        turno.Hora = dto.Hora;
        turno.IdPromocion = dto.IdPromocion;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var turno = await _repository.ObtenerPorId(id);

        if (turno == null)
            return false;

        await _repository.Eliminar(turno);

        return true;
    }
}