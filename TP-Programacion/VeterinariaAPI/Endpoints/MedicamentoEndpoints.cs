using VeterinariaAPI.Logica;
using VeterinariaAPI.Logica.DTOs;

namespace VeterinariaAPI.Endpoints;

public static class MedicamentoEndpoints
{
    public static void MapMedicamentoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/medicamentos", async (IMedicamentoLogica logica) =>
            Results.Ok(await logica.ObtenerTodos()));

        app.MapGet("/medicamentos/{id}", async (int id, IMedicamentoLogica logica) =>
        {
            var m = await logica.ObtenerPorId(id);
            return m == null ? Results.NotFound() : Results.Ok(m);
        });

        app.MapPost("/medicamentos", async (MedicamentoDTO dto, IMedicamentoLogica logica) =>
        {
            await logica.Agregar(dto.Nombre);
            return Results.Created("/medicamentos", new { mensaje = "Medicamento creado" });
        });

        app.MapPut("/medicamentos/{id}", async (int id, MedicamentoDTO dto, IMedicamentoLogica logica) =>
        {
            var ok = await logica.Editar(id, dto.Nombre);
            return ok ? Results.Ok(new { mensaje = "Actualizado correctamente" }) : Results.NotFound();
        });

        app.MapDelete("/medicamentos/{id}", async (int id, IMedicamentoLogica logica) =>
        {
            var ok = await logica.Eliminar(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}