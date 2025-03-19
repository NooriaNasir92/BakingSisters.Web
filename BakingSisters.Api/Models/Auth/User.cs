using BakingSisters.Api.Models.Enum;

namespace BakingSisters.Api.Models.Auth;

public class User : BaseModel
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastLoginDate { get; set; }
} 