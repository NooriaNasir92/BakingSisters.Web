using BakingSisters.Api.Models.Auth;
using BakingSisters.Api.Models.Enum;
using BakingSisters.Api.Services.Auth;
using Xunit;

namespace BakingSisters.Testing.Services;

public class AuthServiceTests : TestBase
{
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(Context, Configuration);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            UserType = UserType.Customer
        };

        // Act
        var result = await _authService.RegisterAsync(user, "Password123!");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.NotEmpty(result.PasswordHash);
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var user1 = new User
        {
            Email = "test@example.com",
            FirstName = "Test1",
            LastName = "User1",
            UserType = UserType.Customer
        };

        var user2 = new User
        {
            Email = "test@example.com",
            FirstName = "Test2",
            LastName = "User2",
            UserType = UserType.Customer
        };

        // Act & Assert
        await _authService.RegisterAsync(user1, "Password123!");
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _authService.RegisterAsync(user2, "Password456!"));
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
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
            Password = "Password123!"
        };

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "WrongPassword123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
            _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WithValidCredentials_ShouldReturnTrue()
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
        var result = await _authService.ValidateCredentialsAsync("test@example.com", "Password123!");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WithInvalidCredentials_ShouldReturnFalse()
    {
        // Act
        var result = await _authService.ValidateCredentialsAsync("nonexistent@example.com", "WrongPassword123!");

        // Assert
        Assert.False(result);
    }
} 