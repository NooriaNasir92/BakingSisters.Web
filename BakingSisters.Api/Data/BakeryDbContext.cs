using BakingSisters.Api.Data.Seeds;
using BakingSisters.Api.Models;
using BakingSisters.Api.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Data;

public class BakeryDbContext : DbContext
{
    public BakeryDbContext(DbContextOptions<BakeryDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderItemCustomization> OrderItemCustomizations { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CustomizationGroup> CustomizationGroups { get; set; }
    public DbSet<CustomizationOption> CustomizationOptions { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<ProductIngredient> ProductIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BakeryDbContext).Assembly);

        // Seed initial data
        modelBuilder.SeedUsers();
        modelBuilder.SeedCategories();
    }
}