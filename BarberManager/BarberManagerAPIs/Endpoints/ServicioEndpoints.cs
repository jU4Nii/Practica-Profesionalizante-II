using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class ServicioEndpoints
{
    public static void MapServicioEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/servicios", async (IServicioLogica logica) =>
        {
            var servicios = await logica.ObtenerTodos();

            return Results.Ok(servicios);
        });

        app.MapGet("/servicios/{id}", async (int id, IServicioLogica logica) =>
        {
            var servicio = await logica.ObtenerPorId(id);

            if (servicio == null)
                return Results.NotFound();

            return Results.Ok(servicio);
        });

        app.MapPost("/servicios", async (ServicioDTO dto, IServicioLogica logica) =>
        {
            var creado = await logica.Agregar(dto);

            if (!creado)
                return Results.BadRequest(new
                {
                    mensaje = "Datos inválidos"
                });

            return Results.Created("/servicios", new
            {
                mensaje = "Servicio creado correctamente"
            });
        });
    }
}