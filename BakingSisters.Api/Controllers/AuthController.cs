using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using BakingSisters.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BakingSisters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            var response = await authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password");
        }
    }

    [HttpPost("guest")]
    public async Task<ActionResult<LoginResponse>> GuestLogin()
    {
        try
        {
            // Create guest user details
            var guestUser = new User
            {
                Email = "guest@bakingsisters.com",
                FirstName = "Guest",
                LastName = "User",
                UserType = UserType.Guest,
                IsActive = true,
                LastLoginDate = DateTime.UtcNow
            };

            // Generate token for guest user
            var token = await authService.GenerateJwtTokenAsync(guestUser);

            // Create response
            var response = new LoginResponse
            {
                UserId = 0, // Guest users don't have permanent IDs
                Email = guestUser.Email,
                FirstName = guestUser.FirstName,
                LastName = guestUser.LastName,
                UserType = Convert.ToInt32(guestUser.UserType),
                Token = token
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to create guest session: {ex.Message}");
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user, [FromQuery] string password)
    {
        try
        {
            var registeredUser = await authService.RegisterAsync(user, password);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = registeredUser.Email }, registeredUser);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("user/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var user = await authService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
} 