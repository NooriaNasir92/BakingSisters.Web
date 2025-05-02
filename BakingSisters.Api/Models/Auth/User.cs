using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using BakingSisters.Api.Models.Enum;

namespace BakingSisters.Api.Models.Auth;

public class User:BaseModel
{

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number")]
    public string? Password { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid WhatsApp number format")]
    public string? WhatsAppNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP code format")]
    public string? ZipCode { get; set; }

    public UserType UserType { get; set; } = UserType.Customer;
    public bool IsActive { get; set; } = true;

    public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;
    [NotMapped]
    public string Token { get; set; } = string.Empty;

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}