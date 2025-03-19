using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BakingSisters.Api.Data;
using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BakingSisters.Api.Services.Auth;

public class AuthService(BakeryDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await GetUserByEmailAsync(request.Email);
        if (user == null || !await ValidateCredentialsAsync(request.Email, request.Password))
            throw new UnauthorizedAccessException("Invalid email or password");

        user.LastLoginDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        var token = await GenerateJwtTokenAsync(user);

        return new LoginResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserType = (int)user.UserType,
            Token = token,
            UserTypeValue = (int)user.UserType
        };
    }

    public async Task<User> RegisterAsync(User user, string password)
    {
        if (await context.Users.AnyAsync(u => u.Email == user.Email))
            throw new InvalidOperationException("Email already exists");

        user.PasswordHash = HashPassword(password);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null)
            return false;

        return VerifyPassword(password, user.PasswordHash);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
} 