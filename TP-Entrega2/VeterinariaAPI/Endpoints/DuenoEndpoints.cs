using VeterinariaAPI.Logica;
using VeterinariaAPI.Logica.DTOs;

namespace VeterinariaAPI.Endpoints;

public static class DuenoEndpoints
{
    public static void MapDuenoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/duenos", async (IDuenoLogica logica) =>
        {
            var duenos = await logica.ObtenerTodos();
            return Results.Ok(duenos);
        });

        app.MapGet("/duenos/{id}", async (int id, IDuenoLogica logica) =>
        {
            var dueno = await logica.ObtenerPorId(id);

            if (dueno == null)
                return Results.NotFound();

            return Results.Ok(dueno);
        });

        app.MapPost("/duenos", async (DuenoDTO dto, IDuenoLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/duenos", new
            {
                mensaje = "Dueño creado"
            });
        });

        app.MapPut("/duenos/{id}", async (int id, DuenoDTO dto, IDuenoLogica logica) =>
        {
            var editado = await logica.Editar(id, dto);

            if (!editado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Dueño actualizado correctamente"
            });
        });

        app.MapDelete("/duenos/{id}", async (int id, IDuenoLogica logica) =>
        {
            var eliminado = await logica.Eliminar(id);

            if (!eliminado)
                return Results.NotFound();

            return Results.NoContent();
        });
    }
}