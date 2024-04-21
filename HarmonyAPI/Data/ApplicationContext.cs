using HarmonyAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HarmonyAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   

        }
    }
}
