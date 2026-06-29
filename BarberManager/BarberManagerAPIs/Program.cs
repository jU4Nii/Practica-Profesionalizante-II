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

            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

            builder.Services.AddScoped<IProductoLogica, ProductoLogica>();

            builder.Services.AddScoped<IPeluqueroRepository, PeluqueroRepository>();

            builder.Services.AddScoped<IPeluqueroLogica, PeluqueroLogica>();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapClienteEndpoints();

            app.MapServicioEndpoints();

            app.MapProductoEndpoints();

            app.MapPeluqueroEndpoints();

            app.Run();
        }
    }
}