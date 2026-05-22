using VeterinariaAPI.Logica;
using VeterinariaAPI.Logica.DTOs;

namespace VeterinariaAPI.Endpoints;

public static class AtencionEndpoints
{
    public static void MapAtencionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/atenciones", async (IAtencionLogica logica) =>
        {
            var atenciones = await logica.ObtenerTodos();
            return Results.Ok(atenciones);
        });

        app.MapGet("/atenciones/{id}", async (int id, IAtencionLogica logica) =>
        {
            var atencion = await logica.ObtenerPorId(id);

            if (atencion == null)
                return Results.NotFound();

            return Results.Ok(atencion);
        });

        app.MapPost("/atenciones", async (AtencionDTO dto, IAtencionLogica logica) =>
        {
            var creada = await logica.Agregar(dto);

            if (!creada)
                return Results.BadRequest(new
                {
                    mensaje = "Fecha inválida"
                });

            return Results.Created("/atenciones", new
            {
                mensaje = "Atención creada"
            });
        });

        app.MapPut("/atenciones/{id}", async (int id, AtencionDTO dto, IAtencionLogica logica) =>
        {
            var editada = await logica.Editar(id, dto);

            if (!editada)
                return Results.BadRequest(new
                {
                    mensaje = "Atención no encontrada o fecha inválida"
                });

            return Results.Ok(new
            {
                mensaje = "Atención actualizada correctamente"
            });
        });

        app.MapDelete("/atenciones/{id}", async (int id, IAtencionLogica logica) =>
        {
            var eliminada = await logica.Eliminar(id);

            if (!eliminada)
                return Results.NotFound();

            return Results.NoContent();
        });
    }
}
