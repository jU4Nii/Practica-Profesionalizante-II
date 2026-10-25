using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class CajaEndpoints
{
    public static void MapCajaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/caja").RequireAuthorization("Admin");

        group.MapGet("", async (ICajaLogica logica) =>
        {
            var cajas = await logica.ObtenerTodos();
            return Results.Ok(cajas);
        });

        group.MapGet("/{id}", async (int id, ICajaLogica logica) =>
        {
            var caja = await logica.ObtenerPorId(id);

            if (caja == null)
                return Results.NotFound();

            return Results.Ok(caja);
        });

        group.MapPost("", async (CajaDTO dto, ICajaLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/caja", new
            {
                mensaje = "Caja creada correctamente"
            });
        });

        group.MapPost("/venta-producto", async (VentaProductoDTO dto, ICajaLogica logica) =>
        {
            var registrada = await logica.RegistrarVentaProducto(dto);

            if (!registrada)
                return Results.BadRequest(new { mensaje = "El producto no está disponible o no hay stock suficiente." });

            return Results.Created("/caja", new { mensaje = "Venta de producto registrada correctamente." });
        });

        group.MapPut("/{id}", async (int id, CajaDTO dto, ICajaLogica logica) =>
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
