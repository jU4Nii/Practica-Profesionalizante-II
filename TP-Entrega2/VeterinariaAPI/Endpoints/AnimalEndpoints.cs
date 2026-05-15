using VeterinariaAPI.Logica;
using VeterinariaAPI.Logica.DTOs;

namespace VeterinariaAPI.Endpoints;

public static class AnimalEndpoints
{
    public static void MapAnimalEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/animales", async (IAnimalLogica logica) =>
        {
            var animales = await logica.ObtenerTodos();
            return Results.Ok(animales);
        });

        app.MapGet("/animales/{id}", async (int id, IAnimalLogica logica) =>
        {
            var animal = await logica.ObtenerPorId(id);

            if (animal == null)
                return Results.NotFound();

            return Results.Ok(animal);
        });

        app.MapPost("/animales", async (AnimalDTO dto, IAnimalLogica logica) =>
        {
            await logica.Agregar(dto);

            return Results.Created("/animales", new
            {
                mensaje = "Animal creado"
            });
        });

        app.MapPut("/animales/{id}", async (int id, AnimalDTO dto, IAnimalLogica logica) =>
        {
            var editado = await logica.Editar(id, dto);

            if (!editado)
                return Results.NotFound();

            return Results.Ok(new
            {
                mensaje = "Animal actualizado correctamente"
            });
        });

        app.MapDelete("/animales/{id}", async (int id, IAnimalLogica logica) =>
        {
            var eliminado = await logica.Eliminar(id);

            if (!eliminado)
                return Results.NotFound();

            return Results.NoContent();
        });
    }
}