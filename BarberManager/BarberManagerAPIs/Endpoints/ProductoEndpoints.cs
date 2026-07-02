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

        app.MapPut("/productos/{id}", async (int id, ProductoDTO dto, IProductoLogica logica) =>
        {
            var editado = await logica.Editar(id, dto);

            if (!editado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Producto actualizado correctamente"
            });
        });

        app.MapDelete("/productos/{id}", async (int id, IProductoLogica logica) =>
        {
            var eliminado = await logica.Eliminar(id);

            if (!eliminado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Producto eliminado correctamente"
            });
        });

    }
}