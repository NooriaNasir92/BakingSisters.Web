using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BakingSisters.Web.Models.Enums;

namespace BakingSisters.Web.Models;

public class User
{
    public int Id { get; set; }

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
    public string? Password { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    [Phone(ErrorMessage = "Invalid WhatsApp number format")]
    public string? WhatsAppNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP code format")]
    public string? ZipCode { get; set; }

    public UserType UserType { get; set; } = UserType.Customer;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public string Token { get; set; } = string.Empty;
}

