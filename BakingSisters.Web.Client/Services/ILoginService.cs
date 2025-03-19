namespace BakingSisters.Web.Client.Services;

public interface ILoginService
{
    Task<LoginResponse> LoginUserAsync(string email, string password);
    Task<LoginResponse> LoginAsGuestAsync();
}