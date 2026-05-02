using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Models;

namespace VeterinariaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Animal> Animales { get; set; }
        public DbSet<Dueno> Duenos { get; set; }
        public DbSet<Atencion> Atenciones { get; set; }
    }
}