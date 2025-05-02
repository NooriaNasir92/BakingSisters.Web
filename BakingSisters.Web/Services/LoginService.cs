using BakingSisters.Api.Data;
using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using BakingSisters.Api.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Web.Services;

// Updated to use BakeryDbContext from API project
public class LoginService(IApiService apiService, ILogger<LoginService> logger, BakeryDbContext context, IAuthService authService) : ILoginService
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

            logger.LogInformation("Attempting to login user {Email}", email);
            var response = await apiService.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
            logger.LogInformation("Login successful for user {Email}", email);
            return MapToUser(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to login user with email: {Email}. Error: {Message}", email, ex.Message);
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
            
            // If API call fails, create a local guest user
            return new User
            {
                Email = "guest@bakingsisters.com",
                FirstName = "Guest",
                LastName = "User",
                UserType = UserType.Guest,
                IsActive = true,
                LastLoginDate = DateTime.UtcNow
            };
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
            LastLoginDate = DateTime.UtcNow,
            Token = response.Token
        };
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        try
        {
            // Ensure certain fields are set
            user.PasswordHash = User.HashPassword(user.Password ?? "");
            string originalPassword = user.Password ?? "";  // Store for token generation
            user.Password = null; // Clear the plain text password
            user.CreatedAt = DateTime.UtcNow;
            user.LastLoginDate = DateTime.UtcNow;
            user.IsActive = true;

            // Add and save the user to the database
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Get the newly created user from the database
            var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            
            if (savedUser != null)
            {
                // Generate a token for the user
                string token = await authService.GenerateJwtTokenAsync(savedUser);
                savedUser.Token = token;
                
                // Create login response for AppState
                AppState.LoggedInUser = savedUser;
            }

            return savedUser ?? new User();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering user: {Error}", ex.Message);
            if (ex.InnerException != null)
            {
                logger.LogError("Inner exception: {Error}", ex.InnerException.Message);
            }
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }
}






