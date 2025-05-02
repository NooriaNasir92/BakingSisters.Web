using BakingSisters.Api.Models.Auth;

namespace BakingSisters.Web;

public static class AppState
{
    /// <summary>
    /// Current Logged In User
    /// </summary>
    public static User? LoggedInUser { get; set; }
    
    /// <summary>
    /// JSON representation of cart data (simulates localStorage)
    /// </summary>
    public static string CartData { get; set; } = string.Empty;
}