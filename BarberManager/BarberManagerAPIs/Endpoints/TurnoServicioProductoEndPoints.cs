using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;
using System.Security.Claims;

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

        app.MapPost("/turnos/items", async (TurnoServicioProductoDTO dto, ClaimsPrincipal user, ITurnoLogica turnoLogica, ITurnoServicioProductoLogica logica) =>
        {
            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                var turno = await turnoLogica.ObtenerPorId(dto.IdTurno);
                if (turno == null)
                    return Results.NotFound();

                if (!int.TryParse(user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var userId) || turno.IdPeluquero != userId)
                    return Results.Forbid();
            }

            var agregado = await logica.Agregar(dto);

            if (!agregado)
                return Results.BadRequest(new { mensaje = "No hay stock suficiente o los datos del producto no son válidos." });

            return Results.Created("/turnos/items", new
            {
                mensaje = "Item agregado correctamente"
            });
        });
    }
}
