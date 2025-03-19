using BakingSisters.Web.Models;

namespace BakingSisters.Web;

public static class AppState
{
    /// <summary>
    /// Current Logged In User
    /// </summary>
    public static User? LoggedInUser { get; set; }
}