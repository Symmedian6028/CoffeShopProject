using Microsoft.EntityFrameworkCore;
using CoffeeShop.Api.Models;

namespace CoffeeShop.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Buraya her tablo için bir DbSet<T> ekleyeceksin
    public DbSet<Coffee> Coffees { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
}
}

