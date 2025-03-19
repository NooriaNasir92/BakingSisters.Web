using BakingSisters.Api.Models.Enum;
using System.Text.Json.Serialization;

namespace BakingSisters.Api.Models.Auth;

public class LoginResponse
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int UserType { get; set; }
    public string Token { get; set; } = string.Empty;
    [JsonPropertyName("userTypeValue")]
    public int UserTypeValue { get; set; }

} 