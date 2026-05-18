using VeterinariaAPI.Logica;
using VeterinariaAPI.Logica.DTOs;

namespace VeterinariaAPI.Endpoints;

public static class RazaEndpoints
{
    public static void MapRazaEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/razas", async (IRazaLogica logica) =>
            Results.Ok(await logica.ObtenerTodos()));

        app.MapGet("/razas/{id}", async (int id, IRazaLogica logica) =>
        {
            var r = await logica.ObtenerPorId(id);
            return r == null ? Results.NotFound() : Results.Ok(r);
        });

        app.MapPost("/razas", async (RazaDTO dto, IRazaLogica logica) =>
        {
            await logica.Agregar(dto.Nombre);
            return Results.Created("/razas", new { mensaje = "Raza creada" });
        });

        app.MapPut("/razas/{id}", async (int id, RazaDTO dto, IRazaLogica logica) =>
        {
            var ok = await logica.Editar(id, dto.Nombre);
            return ok ? Results.Ok(new { mensaje = "Actualizada correctamente" }) : Results.NotFound();
        });

        app.MapDelete("/razas/{id}", async (int id, IRazaLogica logica) =>
        {
            var ok = await logica.Eliminar(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}