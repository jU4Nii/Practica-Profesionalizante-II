using Microsoft.EntityFrameworkCore;


namespace BarberManagerAPIs.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        


    }
}