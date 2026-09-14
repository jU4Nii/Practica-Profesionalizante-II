using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;

namespace BarberManagerAPIs.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (LoginDTO dto, IPeluqueroLogica logica) =>
        {
            var usuario = (await logica.ObtenerTodos()).FirstOrDefault(p =>
                p.EstaActivo &&
                string.Equals(p.Correo, dto.Correo, StringComparison.OrdinalIgnoreCase) &&
                p.Contrasena == dto.Contrasena);

            if (usuario == null)
                return Results.Unauthorized();

            return Results.Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.EsAdmin
            });
        });
    }
}
