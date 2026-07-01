using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class PromocionEndpoints
{
    public static void MapPromocionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/promociones", async (IPromocionLogica logica) =>
        {
            return Results.Ok(await logica.ObtenerTodos());
        });

        app.MapGet("/promociones/{id}", async (int id, IPromocionLogica logica) =>
        {
            var promocion = await logica.ObtenerPorId(id);

            if (promocion == null)
                return Results.NotFound();

            return Results.Ok(promocion);
        });

        app.MapPost("/promociones", async (PromocionDTO dto, IPromocionLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/promociones", new
            {
                mensaje = "Promoción creada correctamente"
            });
        });

        app.MapPost("/promociones/{id}/servicios", async (int id, int idServicio, IPromocionLogica logica) =>
        {
            var resultado = await logica.AsignarServicio(id, idServicio);

            if (!resultado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Servicio asignado correctamente"
            });
        });

        app.MapPost("/promociones/{id}/productos", async (int id, int idProducto, IPromocionLogica logica) =>
        {
            var resultado = await logica.AsignarProducto(id, idProducto);

            if (!resultado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Producto asignado correctamente"
            });
        });

        app.MapDelete("/promociones/{id}/servicios/{idServicio}", async (int id, int idServicio, IPromocionLogica logica) =>
        {
            var resultado = await logica.EliminarServicio(id, idServicio);

            if (!resultado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Servicio eliminado correctamente"
            });
        });

        app.MapDelete("/promociones/{id}/productos/{idProducto}", async (int id, int idProducto, IPromocionLogica logica) =>
        {
            var resultado = await logica.EliminarProducto(id, idProducto);

            if (!resultado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Producto eliminado correctamente"
            });
        });
    }
}