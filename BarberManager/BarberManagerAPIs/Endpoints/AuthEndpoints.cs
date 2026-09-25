using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Logica.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberManagerAPIs.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (LoginDTO dto, IPeluqueroLogica logica, IConfiguration configuration) =>
        {
            var usuario = (await logica.ObtenerTodos()).FirstOrDefault(p =>
                p.EstaActivo &&
                string.Equals(p.Correo, dto.Correo, StringComparison.OrdinalIgnoreCase) &&
                p.Contrasena == dto.Contrasena);

            if (usuario == null)
                return Results.Unauthorized();

            var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Falta configurar Jwt:Key.");
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.EsAdmin ? "Administrador" : "Peluquero")
            };
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);

            return Results.Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.EsAdmin,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }).AllowAnonymous();
    }
}
