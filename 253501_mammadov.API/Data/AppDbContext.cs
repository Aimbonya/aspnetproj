using Microsoft.EntityFrameworkCore;
using mammadov.Domain.Entities;

namespace _253501_mammadov.API.Data
{
    public class AppDbContext : DbContext
    {
        DbContextOptions<AppDbContext> dbContextOptions { get; set; }

        public DbSet<Fruit> Fruits { get; set; }  // Коллекция фруктов
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }
    }
}
