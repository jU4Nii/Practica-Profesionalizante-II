using VeterinariaAPI.Datos;
using VeterinariaAPI.Logica;
using VeterinariaAPI.Repositorios;
using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Endpoints;
namespace VeterinariaAPI
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

            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped<IAnimalLogica, AnimalLogica>();

            builder.Services.AddScoped<IDuenoRepository, DuenoRepository>();
            builder.Services.AddScoped<IDuenoLogica, DuenoLogica>();

            builder.Services.AddScoped<IAtencionRepository, AtencionRepository>();
            builder.Services.AddScoped<IAtencionLogica, AtencionLogica>();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            

            

            app.MapAnimalEndpoints();

            app.MapDuenoEndpoints();

            app.MapAtencionEndpoints();

            app.Run();
        }
    }
}