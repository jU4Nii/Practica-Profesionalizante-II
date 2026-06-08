using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class ProductoEndpoints
{
    public static void MapProductoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/productos", async (IProductoLogica logica) =>
        {
            var productos = await logica.ObtenerTodos();

            return Results.Ok(productos);
        });

        app.MapGet("/productos/{id}", async (int id, IProductoLogica logica) =>
        {
            var producto = await logica.ObtenerPorId(id);

            if (producto == null)
                return Results.NotFound();

            return Results.Ok(producto);
        });

        app.MapPost("/productos", async (ProductoDTO dto, IProductoLogica logica) =>
        {
            var creado = await logica.Agregar(dto);

            if (!creado)
                return Results.BadRequest(new
                {
                    mensaje = "Datos inválidos"
                });

            return Results.Created("/productos", new
            {
                mensaje = "Producto creado correctamente"
            });
        });
    }
}