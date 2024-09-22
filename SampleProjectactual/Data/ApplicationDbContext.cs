using Microsoft.EntityFrameworkCore;
using SampleProjectactual.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; } 
    public DbSet<OrderItem> OrderItems { get; set; } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure decimal precision and scale
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)"); // 18 digits in total, 2 after the decimal point

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasColumnType("decimal(18,2)"); // 18 digits in total, 2 after the decimal point


        modelBuilder.Entity<Product>().HasData(
            new Product { pid = 1, name = "Product 1", quantity = 10, price = 100, description = "Description of Product 1" },
            new Product { pid = 2, name = "Product 2", quantity = 5, price = 200, description = "Description of Product 2" },
            new Product { pid = 3, name = "Product 3", quantity = 20, price = 150, description = "Description of Product 3" },
            new Product { pid = 4, name = "Product 4", quantity = 1000, price = 20, description = "Description of Product 4" },
            new Product { pid = 5, name = "Product 5", quantity = 120, price = 570, description = "Description of Product 5" }
        // Add more products as needed
        );

        // Add any additional configuration for Order and OrderItem models if needed
    }
}
