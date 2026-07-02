using BarberManagerAPIs.Datos;
using BarberManagerAPIs.Endpoints;
using BarberManagerAPIs.Logica;
using BarberManagerAPIs.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
namespace BarberManagerAPIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            app.MapClienteEndpoints();

            app.MapServicioEndpoints();

            app.MapTurnoEndpoints();

            app.MapProductoEndpoints();

            app.MapPeluqueroEndpoints();

            app.MapEstadisticaEndpoints();

            app.MapCajaEndpoints();

            app.MapPromocionEndpoints();

            app.MapTurnoServicioProductoEndpoints();

         

            app.Run();
        }
    }
}