using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;
using System.Security.Claims;

namespace BarberManagerAPIs.Endpoints;

public static class TurnoEndpoints
{
    public static void MapTurnoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/turnos", async (ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var esAdmin = user.IsInRole("Administrador");
            if (esAdmin)
            {
                var turnos = await logica.ObtenerTodos();
                return Results.Ok(turnos);
            }

            if (int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                var turnos = await logica.ObtenerPorPeluquero(userId);
                return Results.Ok(turnos);
            }

            return Results.Forbid();
        });

        app.MapGet("/turnos/{id}", async (int id, ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var turno = await logica.ObtenerPorId(id);

            if (turno == null)
                return Results.NotFound();

            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) || turno.IdPeluquero != userId)
                    return Results.Forbid();
            }

            return Results.Ok(turno);
        });

        app.MapPost("/turnos", async (TurnoDTO dto, ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                if (int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                {
                    dto.IdPeluquero = userId; // Garantiza que el turno se cree para el peluquero autenticado
                }
                else
                {
                    return Results.Forbid();
                }
            }

            var turnoCreado = await logica.Agregar(dto);

            if (turnoCreado == null)
                return Results.Conflict(new
                {
                    mensaje = "El peluquero ya tiene un turno en ese horario."
                });

            return Results.Created($"/turnos/{turnoCreado.Id}", new
            {
                id = turnoCreado.Id,
                mensaje = "Turno creado correctamente"
            });
        });

        app.MapPut("/turnos/{id}", async (int id, TurnoDTO dto, ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var turno = await logica.ObtenerPorId(id);
            if (turno == null)
                return Results.NotFound();

            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) || turno.IdPeluquero != userId)
                    return Results.Forbid();

                dto.IdPeluquero = userId; // No puede transferirlo a otro peluquero
            }

            var editado = await logica.Editar(id, dto);

            if (!editado)
                return Results.Conflict(new
                {
                    mensaje = "No se pudo reprogramar el turno porque el peluquero ya tiene otro turno en ese horario."
                });

            return Results.Ok(new
            {
                mensaje = "Turno reprogramado correctamente"
            });
        });

        app.MapDelete("/turnos/{id}", async (int id, ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var turno = await logica.ObtenerPorId(id);
            if (turno == null)
                return Results.NotFound();

            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) || turno.IdPeluquero != userId)
                    return Results.Forbid();
            }

            var eliminado = await logica.Eliminar(id);

            if (!eliminado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Turno cancelado correctamente"
            });
        });

        app.MapDelete("/turnos/{id}/permanente", async (int id, ITurnoLogica logica) =>
        {
            var eliminado = await logica.EliminarDefinitivamente(id);

            if (!eliminado)
                return Results.BadRequest(new { mensaje = "Solo se pueden borrar definitivamente turnos cancelados." });

            return Results.Ok(new { mensaje = "Turno eliminado definitivamente." });
        }).RequireAuthorization("Admin");

        app.MapGet("/turnos/fecha/{fecha}", async (DateTime fecha, ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var turnos = await logica.ObtenerPorFecha(fecha);
            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                if (int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                {
                    turnos = turnos.Where(t => t.IdPeluquero == userId).ToList();
                }
                else
                {
                    return Results.Forbid();
                }
            }
            return Results.Ok(turnos);
        });

        app.MapGet("/turnos/peluquero/{idPeluquero}", async (int idPeluquero, ClaimsPrincipal user, ITurnoLogica logica) =>
        {
            var esAdmin = user.IsInRole("Administrador");
            if (!esAdmin)
            {
                if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) || idPeluquero != userId)
                    return Results.Forbid();
            }

            var turnos = await logica.ObtenerPorPeluquero(idPeluquero);
            return Results.Ok(turnos);
        });
    }
}
