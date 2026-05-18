using VeterinariaAPI.Entidades;
using VeterinariaAPI.Logica.DTOs;
using VeterinariaAPI.Repositorios;

namespace VeterinariaAPI.Logica;

public interface IAtencionLogica
{
    Task<List<Atencion>> ObtenerTodos();
    Task<Atencion?> ObtenerPorId(int id);
    Task<bool> Agregar(AtencionDTO dto);
    Task<bool> Editar(int id, AtencionDTO dto);
    Task<bool> Eliminar(int id);
}

public class AtencionLogica : IAtencionLogica
{
    private readonly IAtencionRepository _repository;

    public AtencionLogica(IAtencionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Atencion>> ObtenerTodos()
    {
        return await _repository.ObtenerTodos();
    }

    public async Task<Atencion?> ObtenerPorId(int id)
    {
        return await _repository.ObtenerPorId(id);
    }

    public async Task<bool> Agregar(AtencionDTO dto)
    {
        if (dto.Fecha > DateTime.Now || dto.Fecha < DateTime.Now.AddDays(-30))
            return false;

        Atencion atencion = new Atencion
        {
            AnimalId = dto.AnimalId,
            Motivo = dto.Motivo,
            TratamientoId = dto.TratamientoId,
            MedicamentosIds = dto.MedicamentosIds,
            Fecha = dto.Fecha
        };

        await _repository.Agregar(atencion);

        return true;
    }

    public async Task<bool> Editar(int id, AtencionDTO dto)
    {
        var atencion = await _repository.ObtenerPorId(id);

        if (atencion == null)
            return false;

        if (dto.Fecha > DateTime.Now || dto.Fecha < DateTime.Now.AddDays(-30))
            return false;

        atencion.AnimalId = dto.AnimalId;
        atencion.Motivo = dto.Motivo;
        atencion.TratamientoId = dto.TratamientoId;
        atencion.MedicamentosIds = dto.MedicamentosIds;
        atencion.Fecha = dto.Fecha;

        await _repository.Guardar();

        return true;
    }

    public async Task<bool> Eliminar(int id)
    {
        var atencion = await _repository.ObtenerPorId(id);

        if (atencion == null)
            return false;

        await _repository.Eliminar(atencion);

        return true;
    }
}