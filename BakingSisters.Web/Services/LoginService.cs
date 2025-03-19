using BakingSisters.Web.Data;
using BakingSisters.Web.Models;
using BakingSisters.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Web.Services;

public class LoginService(IApiService apiService, ILogger<LoginService> logger, ApplicationDbContext context) : ILoginService
{
    public async Task<User> LoginUserAsync(string email, string password)
    {
        try
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await apiService.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
            return MapToUser(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to login user with email: {Email}", email);
            throw;
        }
    }

    public async Task<User> LoginAsGuestAsync()
    {
        try
        {
            var response = await apiService.PostAsync<object, LoginResponse>("api/auth/guest", new { });
            return MapToUser(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to login as guest");
            throw;
        }
    }

    private User MapToUser(LoginResponse response)
    {
        return new User
        {
            Email = response.Email,
            FirstName = response.FirstName,
            LastName = response.LastName,
            UserType = (UserType)response.UserTypeValue,
            Token = response.Token
        };
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return await context.Users.FirstOrDefaultAsync(u => u.FirstName == user.FirstName && u.Email == user.Email) ?? new User();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }
}






