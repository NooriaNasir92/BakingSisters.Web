using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using BakingSisters.Api.Services.Auth;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BakingSisters.Testing.Services;

public class LoginServiceTests : TestBase
{
    private readonly IAuthService _authService;
    private readonly Mock<ILogger<LoginServiceTests>> _loggerMock;

    public LoginServiceTests()
    {
        _authService = new AuthService(Context, Configuration);
        _loggerMock = new Mock<ILogger<LoginServiceTests>>();
    }

    [Fact]
    public async Task LoginUserAsync_WithValidCredentials_ShouldReturnLoginResponse()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            UserType = UserType.Customer
        };
        await _authService.RegisterAsync(user, "Password123!");

        // Act
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.UserType, result.UserType);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginUserAsync_WithInvalidEmail_ShouldThrowException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginUserAsync_WithInvalidPassword_ShouldThrowException()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            UserType = UserType.Customer
        };
        await _authService.RegisterAsync(user, "Password123!");

        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginUserAsync_WithAdminCredentials_ShouldReturnAdminResponse()
    {
        // Arrange
        var adminUser = new User
        {
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            UserType = UserType.Admin
        };
        await _authService.RegisterAsync(adminUser, "AdminPass123!");

        var loginRequest = new LoginRequest
        {
            Email = "admin@example.com",
            Password = "AdminPass123!"
        };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(UserType.Admin, result.UserType);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginUserAsync_WithGuestCredentials_ShouldReturnGuestResponse()
    {
        // Arrange
        var guestUser = new User
        {
            Email = "guest@example.com",
            FirstName = "Guest",
            LastName = "User",
            UserType = UserType.Guest
        };
        await _authService.RegisterAsync(guestUser, "GuestPass123!");

        var loginRequest = new LoginRequest
        {
            Email = "guest@example.com",
            Password = "GuestPass123!"
        };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(UserType.Guest, result.UserType);
        Assert.NotEmpty(result.Token);
    }

    [Theory]
    [InlineData("", "Password123!")]
    [InlineData("test@example.com", "")]
    [InlineData("", "")]
    [InlineData(null, "Password123!")]
    [InlineData("test@example.com", null)]
    public async Task LoginUserAsync_WithInvalidInput_ShouldThrowException(string email, string password)
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginUserAsync_WithInactiveUser_ShouldThrowException()
    {
        // Arrange
        var inactiveUser = new User
        {
            Email = "inactive@example.com",
            FirstName = "Inactive",
            LastName = "User",
            UserType = UserType.Customer,
            IsActive = false
        };
        await _authService.RegisterAsync(inactiveUser, "Password123!");

        // Manually deactivate the user
        inactiveUser.IsActive = false;
        Context.Users.Update(inactiveUser);
        await Context.SaveChangesAsync();

        var loginRequest = new LoginRequest
        {
            Email = "inactive@example.com",
            Password = "Password123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginUserAsync_WithValidCredentials_ShouldUpdateLastLoginDate()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            UserType = UserType.Customer,
            LastLoginDate = DateTime.UtcNow.AddDays(-1)
        };
        await _authService.RegisterAsync(user, "Password123!");

        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        // Act
        await _authService.LoginAsync(loginRequest);

        // Assert
        var updatedUser = await Context.Users.FindAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.True(updatedUser.LastLoginDate > user.LastLoginDate);
    }

    [Fact]
    public async Task LoginUserAsync_MultipleFailedAttempts_ShouldStillAllowValidLogin()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            UserType = UserType.Customer
        };
        await _authService.RegisterAsync(user, "Password123!");

        // Failed attempts
        for (int i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _authService.LoginAsync(new LoginRequest
                {
                    Email = "test@example.com",
                    Password = $"WrongPassword{i}!"
                }));
        }

        // Valid login
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.NotEmpty(result.Token);
    }
} 