using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class TurnoServicioProductoEndpoints
{
    public static void MapTurnoServicioProductoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/turnos/items", async (ITurnoServicioProductoLogica logica) =>
        {
            var items = await logica.ObtenerTodos();
            return Results.Ok(items);
        });

        app.MapGet("/turnos/items/{id}", async (int id, ITurnoServicioProductoLogica logica) =>
        {
            var item = await logica.ObtenerPorId(id);

            if (item == null)
                return Results.NotFound();

            return Results.Ok(item);
        });

        app.MapPost("/turnos/items", async (TurnoServicioProductoDTO dto, ITurnoServicioProductoLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/turnos/items", new
            {
                mensaje = "Item agregado correctamente"
            });
        });
    }
}