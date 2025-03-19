using System.Security.Cryptography;
using System.Text;
using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Data.Seeds;

public static class UserSeed
{
    // Using a static date for seeding
    //private static readonly DateTime DefaultDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime DefaultDate = DateTime.UtcNow;

    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Email = "admin@bakingsisters.com",
            PasswordHash = HashPassword("admin"),
            FirstName = "Admin",
            LastName = "User",
            UserType = UserType.Admin,
            IsActive = true,
            LastLoginDate = DefaultDate
        });
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
} 