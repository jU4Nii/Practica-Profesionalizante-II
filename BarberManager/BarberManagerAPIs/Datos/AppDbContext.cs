using Microsoft.EntityFrameworkCore;
using VeterinariaAPI.Entidades;

namespace VeterinariaAPI.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Animal> Animales => Set<Animal>();
        public DbSet<Atencion> Atenciones => Set<Atencion>();
        public DbSet<Dueno> Duenos => Set<Dueno>();
        public DbSet<Raza> Razas => Set<Raza>();
        public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
        public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();


    }
}