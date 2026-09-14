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
    private readonly IEstadisticaLogica _estadisticaLogica;

    public CajaLogica(ICajaRepository repository, IEstadisticaLogica estadisticaLogica)
    {
        _repository = repository;
        _estadisticaLogica = estadisticaLogica;
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
            Fecha = dto.Fecha,
            Monto = dto.Monto,
            Concepto = dto.Concepto,
            MetodoPago = dto.MetodoPago,
            EsIngreso = dto.EsIngreso
        };

        await _repository.Agregar(caja);

        if (dto.EsIngreso && dto.Concepto == "Venta de producto")
            await _estadisticaLogica.RegistrarVenta(dto.Fecha);
    }

    public async Task<bool> Editar(int id, CajaDTO dto)
    {
        var caja = await _repository.ObtenerPorId(id);

        if (caja == null)
            return false;

        caja.Fecha = dto.Fecha;
        caja.Monto = dto.Monto;
        caja.Concepto = dto.Concepto;
        caja.MetodoPago = dto.MetodoPago;
        caja.EsIngreso = dto.EsIngreso;

        await _repository.Guardar();

        return true;
    }
}
