using BakingSisters.Web.Models;

namespace BakingSisters.Web.Services;

public interface ILoginService
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<User> RegisterUserAsync(User user);

    /// <summary>
    /// Login an existing user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<User> LoginUserAsync(string email, string password);

    /// <summary>
    /// Get All users
    /// </summary>
    /// <returns>return all Users</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// log in as a guest
    /// </summary>
    /// <returns></returns>
    Task<User> LoginAsGuestAsync();
}