using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Data.Seeds;

public static class UserSeed
{
    public static async Task SeedUsersAsync(BakeryDbContext context)
    {
        // Only seed if the database is empty
        if (!await context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@bakingsisters.com",
                PasswordHash = User.HashPassword("Admin123!"),
                Password = null, // Clear plain text password
                PhoneNumber = "123-456-7890",
                UserType = UserType.Admin,
                CreatedAt = DateTime.UtcNow
            };

            var customer = new User
            {
                FirstName = "Test",
                LastName = "Customer",
                Email = "customer@example.com",
                PasswordHash = User.HashPassword("Customer123!"),
                Password = null, // Clear plain text password
                PhoneNumber = "987-654-3210",
                StreetAddress = "123 Main St",
                City = "Lahore",
                ZipCode = "12345",
                UserType = UserType.Customer,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddRangeAsync(adminUser, customer);
            await context.SaveChangesAsync();
        }
    }
} 