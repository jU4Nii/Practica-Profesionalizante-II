using VeterinariaAPI.Logica;
using VeterinariaAPI.Logica.DTOs;

namespace VeterinariaAPI.Endpoints;

public static class TratamientoEndpoints
{
    public static void MapTratamientoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/tratamientos", async (ITratamientoLogica logica) =>
            Results.Ok(await logica.ObtenerTodos()));

        app.MapGet("/tratamientos/{id}", async (int id, ITratamientoLogica logica) =>
        {
            var t = await logica.ObtenerPorId(id);
            return t == null ? Results.NotFound() : Results.Ok(t);
        });

        app.MapPost("/tratamientos", async (TratamientoDTO dto, ITratamientoLogica logica) =>
        {
            await logica.Agregar(dto.Descripcion);
            return Results.Created("/tratamientos", new { mensaje = "Tratamiento creado" });
        });

        app.MapPut("/tratamientos/{id}", async (int id, TratamientoDTO dto, ITratamientoLogica logica) =>
        {
            var ok = await logica.Editar(id, dto.Descripcion);
            return ok ? Results.Ok(new { mensaje = "Actualizado correctamente" }) : Results.NotFound();
        });

        app.MapDelete("/tratamientos/{id}", async (int id, ITratamientoLogica logica) =>
        {
            var ok = await logica.Eliminar(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}