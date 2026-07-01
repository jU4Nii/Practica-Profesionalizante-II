using BarberManagerAPIs.Entidades;
using BarberManagerAPIs.Logica.DTOs;
using BarberManagerAPIs.Repositorios;

namespace BarberManagerAPIs.Logica;

public interface ICajaLogica
{
    Task<List<Caja>> ObtenerTodos();
    Task<Caja?> ObtenerPorId(int id);
    Task Agregar(CajaDTO dto);
    Task<bool> Editar(int id, CajaDTO dto);
}

public class CajaLogica : ICajaLogica
{
    private readonly ICajaRepository _repository;

    public CajaLogica(ICajaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Caja>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Caja?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task Agregar(CajaDTO dto)
    {
        Caja caja = new Caja
        {
            FechaInicio = dto.FechaInicio,
            FechaCierre = dto.FechaCierre,
            Ingresos = dto.Ingresos,
            Egresos = dto.Egresos
        };

        await _repository.Agregar(caja);
    }

    public async Task<bool> Editar(int id, CajaDTO dto)
    {
        var caja = await _repository.ObtenerPorId(id);

        if (caja == null)
            return false;

        caja.FechaInicio = dto.FechaInicio;
        caja.FechaCierre = dto.FechaCierre;
        caja.Ingresos = dto.Ingresos;
        caja.Egresos = dto.Egresos;

        await _repository.Guardar();

        return true;
    }
}