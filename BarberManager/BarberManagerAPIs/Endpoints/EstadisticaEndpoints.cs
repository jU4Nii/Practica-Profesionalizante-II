using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class EstadisticaEndpoints
{
    public static void MapEstadisticaEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/estadisticas", async (IEstadisticaLogica logica) =>
        {
            var estadisticas = await logica.ObtenerTodos();
            return Results.Ok(estadisticas);
        });

        app.MapGet("/estadisticas/{id}", async (int id, IEstadisticaLogica logica) =>
        {
            var estadistica = await logica.ObtenerPorId(id);

            if (estadistica == null)
                return Results.NotFound();

            return Results.Ok(estadistica);
        });

        app.MapPost("/estadisticas", async (EstadisticaDTO dto, IEstadisticaLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/estadisticas", new
            {
                mensaje = "Estadística creada correctamente"
            });
        });
    }
}