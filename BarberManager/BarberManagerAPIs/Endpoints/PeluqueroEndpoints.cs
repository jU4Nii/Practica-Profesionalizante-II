using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class PeluqueroEndpoints
{
    public static void MapPeluqueroEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/peluqueros", async (IPeluqueroLogica logica) =>
        {
            var peluqueros = await logica.ObtenerTodos();
            return Results.Ok(peluqueros);
        });

        app.MapGet("/peluqueros/{id}", async (int id, IPeluqueroLogica logica) =>
        {
            var peluquero = await logica.ObtenerPorId(id);

            if (peluquero == null)
                return Results.NotFound();

            return Results.Ok(peluquero);
        });

        app.MapPost("/peluqueros", async (PeluqueroDTO dto, IPeluqueroLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/peluqueros", new
            {
                mensaje = "Peluquero creado correctamente"
            });
        });
    }
}