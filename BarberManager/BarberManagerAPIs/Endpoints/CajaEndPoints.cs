using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class CajaEndpoints
{
    public static void MapCajaEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/caja", async (ICajaLogica logica) =>
        {
            var cajas = await logica.ObtenerTodos();
            return Results.Ok(cajas);
        });

        app.MapGet("/caja/{id}", async (int id, ICajaLogica logica) =>
        {
            var caja = await logica.ObtenerPorId(id);

            if (caja == null)
                return Results.NotFound();

            return Results.Ok(caja);
        });

        app.MapPost("/caja", async (CajaDTO dto, ICajaLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/caja", new
            {
                mensaje = "Caja creada correctamente"
            });
        });

        app.MapPut("/caja/{id}", async (int id, CajaDTO dto, ICajaLogica logica) =>
        {
            var editada = await logica.Editar(id, dto);

            if (!editada)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Caja actualizada correctamente"
            });
        });
    }
}