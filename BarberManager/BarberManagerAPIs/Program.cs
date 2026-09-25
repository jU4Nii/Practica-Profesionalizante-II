using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Endpoints;
using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace BarberManagerAPIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Falta configurar Jwt:Key.");
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
                options.AddPolicy("Admin", policy => policy.RequireRole("Administrador"));
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

            builder.Services.AddScoped<IClienteLogica, ClienteLogica>();

            builder.Services.AddScoped<IServicioRepository, ServicioRepository>();

            builder.Services.AddScoped<IServicioLogica, ServicioLogica>();

            builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
            builder.Services.AddScoped<ITurnoLogica, TurnoLogica>();

            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

            builder.Services.AddScoped<IProductoLogica, ProductoLogica>();

            builder.Services.AddScoped<IPeluqueroRepository, PeluqueroRepository>();

            builder.Services.AddScoped<IPeluqueroLogica, PeluqueroLogica>();

            builder.Services.AddScoped<IEstadisticaRepository, EstadisticaRepository>();

            builder.Services.AddScoped<IEstadisticaLogica, EstadisticaLogica>();

            builder.Services.AddScoped<ICajaRepository, CajaRepository>();

            builder.Services.AddScoped<ICajaLogica, CajaLogica>();

            builder.Services.AddScoped<IPromocionRepository, PromocionRepository>();

            builder.Services.AddScoped<IPromocionLogica, PromocionLogica>();

            builder.Services.AddScoped<ITurnoServicioProductoRepository, TurnoServicioProductoRepository>();

            builder.Services.AddScoped<ITurnoServicioProductoLogica, TurnoServicioProductoLogica>();

            builder.Services.AddScoped<ICajaLogica, CajaLogica>();

            builder.Services.AddScoped<ICajaRepository, CajaRepository>();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapClienteEndpoints();

            app.MapServicioEndpoints();

            app.MapTurnoEndpoints();

            app.MapProductoEndpoints();

            app.MapPeluqueroEndpoints();

            app.MapAuthEndpoints();

            app.MapEstadisticaEndpoints();

            app.MapCajaEndpoints();

            app.MapPromocionEndpoints();

            app.MapTurnoServicioProductoEndpoints();

         

            app.Run();
        }
    }
}
