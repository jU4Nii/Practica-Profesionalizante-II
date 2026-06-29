using BarberManagerAPIs.Entidades;
using Microsoft.EntityFrameworkCore;


namespace BarberManagerAPIs.Datos
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Servicio> Servicios { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Peluquero> Peluqueros { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        


    }
}