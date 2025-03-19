using BakingSisters.Api.Models.Auth;
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