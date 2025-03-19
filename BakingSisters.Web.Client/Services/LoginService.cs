namespace BakingSisters.Web.Client.Services;


public class LoginService(IApiService apiService, ILogger<LoginService> logger) : ILoginService
{
    public async Task<LoginResponse> LoginUserAsync(string email, string password)
    {
        try
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            return await apiService.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to login user with email: {Email}", email);
            throw;
        }
    }

    public async Task<LoginResponse> LoginAsGuestAsync()
    {
        try
        {
            return await apiService.PostAsync<object, LoginResponse>("api/auth/guest", new { });
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to login as guest");
            throw;
        }
    }
}
