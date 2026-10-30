using CarritoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarritoAPI.Data
{
    public class CarritoContext : DbContext
    {
        public CarritoContext(DbContextOptions<CarritoContext> options) : base(options) { }
        public DbSet<Producto> Productos { get; set; }
    }
}
