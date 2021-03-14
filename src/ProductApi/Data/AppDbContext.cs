using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using System.Collections.Generic;

namespace ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasData(new List<Product>
                {
                    new Product(1, "Product 1", 1000),
                    new Product(2, "Product 2", 2000),
                    new Product(3, "Product 3", 3000)
                });
        }

        public DbSet<Product> Products { get; set; }
    }
}
