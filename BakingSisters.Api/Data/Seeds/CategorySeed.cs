using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Data.Seeds;

public static class CategorySeed
{
    public static void SeedCategories(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Cake", Description = "Includes birthday cakes, wedding cakes, and custom cakes", CreatedAt = DateTime.UtcNow,CreatedBy = "admin"},
            new Category { Id = 2, Name = "Cupcakes & Muffins", Description = "Includes Various types of cupcakes and muffins", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 3, Name = "Cookie", Description = "Includes birthday cookies, wedding cookies, and custom cookies", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 4, Name = "Pies & Tarts", Description = "Includes Fruit pies, cream pies, and assorted tarts", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 5, Name = "Pastry", Description = "Includes birthday pastries, wedding pastries, and custom pastries", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 6, Name = "Pie", Description = "Includes birthday pies, wedding pies, and custom pies", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 7, Name = "Dessert", Description = "Includes birthday desserts, wedding desserts, and custom desserts", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 8, Name = "Beverage", Description = "Includes birthday beverages, wedding beverages, and custom beverages", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 9, Name = "Savory", Description = "Includes birthday savory, wedding savory, and custom savory", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 10, Name = "Bread", Description = "Includes Different kinds of bread like sourdough, baguette, focaccia", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 11, Name = "Donuts", Description = "Includes Glazed donuts, filled donuts, and mini donuts", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 12, Name = "Scones & Biscuits", Description = "Includes Sweet and savory scones, buttermilk biscuits", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 13, Name = "Gluten-Free & Special Diet", Description = "Includes Gluten-free, keto, and vegan baked goods", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" },
            new Category { Id = 14, Name = "Gift Boxes & Sets", Description = "Assorted gift boxes for cookies, pastries, and more", CreatedAt = DateTime.UtcNow, CreatedBy = "admin" }
        );

    }
}