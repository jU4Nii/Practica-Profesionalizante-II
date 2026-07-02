using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class TurnoEndpoints
{
    public static void MapTurnoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/turnos", async (ITurnoLogica logica) =>
        {
            var turnos = await logica.ObtenerTodos();
            return Results.Ok(turnos);
        });

        app.MapGet("/turnos/{id}", async (int id, ITurnoLogica logica) =>
        {
            var turno = await logica.ObtenerPorId(id);

            if (turno == null)
                return Results.NotFound();

            return Results.Ok(turno);
        });

        app.MapPost("/turnos", async (TurnoDTO dto, ITurnoLogica logica) =>
        {
            var creado = await logica.Agregar(dto);

            if (!creado)
                return Results.Conflict(new
                {
                    mensaje = "El peluquero ya tiene un turno en ese horario."
                });

            return Results.Created("/turnos", new
            {
                mensaje = "Turno creado correctamente"
            });
        });

        app.MapPut("/turnos/{id}", async (int id, TurnoDTO dto, ITurnoLogica logica) =>
        {
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

        app.MapDelete("/turnos/{id}", async (int id, ITurnoLogica logica) =>
        {
            var eliminado = await logica.Eliminar(id);

            if (!eliminado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Turno cancelado correctamente"
            });
        });

        app.MapGet("/turnos/fecha/{fecha}", async (DateTime fecha, ITurnoLogica logica) =>
        {
            var turnos = await logica.ObtenerPorFecha(fecha);
            return Results.Ok(turnos);
        });

        app.MapGet("/turnos/peluquero/{idPeluquero}", async (int idPeluquero, ITurnoLogica logica) =>
        {
            var turnos = await logica.ObtenerPorPeluquero(idPeluquero);
            return Results.Ok(turnos);
        });
    }
}