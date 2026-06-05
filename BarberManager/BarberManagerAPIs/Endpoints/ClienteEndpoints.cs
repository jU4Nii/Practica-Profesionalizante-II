using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/clientes", async (IClienteLogica logica) =>
        {
            var clientes = await logica.ObtenerTodos();

            return Results.Ok(clientes);
        });

        app.MapGet("/clientes/{id}", async (int id, IClienteLogica logica) =>
        {
            var cliente = await logica.ObtenerPorId(id);

            if (cliente == null)
                return Results.NotFound();

            return Results.Ok(cliente);
        });

        app.MapPost("/clientes", async (ClienteDTO dto, IClienteLogica logica) =>
        {
            var creado = await logica.Agregar(dto);

            if (!creado)
                return Results.BadRequest(new
                {
                    mensaje = "Datos inválidos"
                });

            return Results.Created("/clientes", new
            {
                mensaje = "Cliente creado correctamente"
            });
        });
    }
}
