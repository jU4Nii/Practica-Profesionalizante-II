using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class ProveedorEndpoints
{
    public static void MapProveedorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/proveedores").RequireAuthorization("Admin");

        group.MapGet("", async (IProveedorLogica logica) =>
        {
            var proveedores = await logica.ObtenerTodos();

            return Results.Ok(proveedores);
        });

        group.MapGet("/{id}", async (int id, IProveedorLogica logica) =>
        {
            var proveedor = await logica.ObtenerPorId(id);

            if (proveedor == null)
                return Results.NotFound();

            return Results.Ok(proveedor);
        });

        group.MapPost("", async (ProveedorDTO dto, IProveedorLogica logica) =>
        {
            var proveedor = await logica.Agregar(dto);

            if (proveedor == null)
                return Results.BadRequest(new
                {
                    mensaje = "Datos inválidos"
                });

            return Results.Created($"/proveedores/{proveedor.Id}", new
            {
                id = proveedor.Id,
                nombre = proveedor.Nombre,
                mensaje = "Proveedor creado correctamente"
            });
        });

        group.MapPut("/{id}", async (int id, ProveedorDTO dto, IProveedorLogica logica) =>
        {
            var existente = await logica.ObtenerPorId(id);
            if (existente == null)
                return Results.NotFound();

            var editado = await logica.Editar(id, dto);
            return editado
                ? Results.Ok(new { mensaje = "Proveedor actualizado correctamente" })
                : Results.BadRequest(new { mensaje = "No se pudo actualizar el proveedor" });
        });
    }
}
