using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Animal> Animales { get; set; }
        public DbSet<Dueno> Duenos { get; set; }
        public DbSet<Atencion> Atenciones { get; set; }
    }
}