using BakingSisters.Api.Models.Auth;

namespace BakingSisters.Api.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<User> RegisterAsync(User user, string password);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<User?> GetUserByEmailAsync(string email);
    Task<string> GenerateJwtTokenAsync(User user);
} 