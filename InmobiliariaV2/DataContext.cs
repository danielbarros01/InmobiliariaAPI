using InmobiliariaV2.Models;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaV2
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Propietario> Propietarios { get; set; }
    }
}
