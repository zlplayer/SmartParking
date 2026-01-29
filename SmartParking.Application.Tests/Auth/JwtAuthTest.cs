using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Moq;
using SmartParking.Application.Auth;
using SmartParking.Application.Dtos;
using Xunit;

namespace SmartParking.Application.Tests.Auth;

[TestSubject(typeof(JwtAuth))]
public class JwtAuthTest
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly JwtAuth _jwtAuth;

    public JwtAuthTest()
    {
        _configurationMock = new Mock<IConfiguration>();

        // Setup configuration values
        _configurationMock.Setup(x => x["Jwt:Key"]).Returns("SuperSecretKeyForTestingPurposes123456789!");
        _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

        _jwtAuth = new JwtAuth(_configurationMock.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            RoleName = "User"
        };

        // Act
        var token = _jwtAuth.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDto
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            RoleName = "Admin"
        };

        // Act
        var token = _jwtAuth.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.Equal(userId.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal("testuser", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal("test@example.com", jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        Assert.Equal("Admin", jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectIssuerAndAudience()
    {
        // Arrange
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            RoleName = "User"
        };

        // Act
        var token = _jwtAuth.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.Equal("TestIssuer", jwtToken.Issuer);
        Assert.Contains("TestAudience", jwtToken.Audiences);
    }

    [Fact]
    public void GenerateToken_ShouldHaveExpirationTime()
    {
        // Arrange
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            RoleName = "User"
        };

        // Act
        var token = _jwtAuth.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        // Token should expire in approximately 2 hours
        var expectedExpiry = DateTime.UtcNow.AddHours(2);
        var actualExpiry = jwtToken.ValidTo;

        // Allow 1 minute tolerance for test execution time
        Assert.True(Math.Abs((expectedExpiry - actualExpiry).TotalMinutes) < 1);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateUniqueTokensForDifferentUsers()
    {
        // Arrange
        var user1 = new UserDto
        {
            Id = Guid.NewGuid(),
            Username = "user1",
            Email = "user1@example.com",
            RoleName = "User"
        };

        var user2 = new UserDto
        {
            Id = Guid.NewGuid(),
            Username = "user2",
            Email = "user2@example.com",
            RoleName = "User"
        };

        // Act
        var token1 = _jwtAuth.GenerateToken(user1);
        var token2 = _jwtAuth.GenerateToken(user2);

        // Assert
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GenerateToken_TokenShouldBeDecodable()
    {
        // Arrange
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            RoleName = "User"
        };

        // Act
        var token = _jwtAuth.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();

        // Assert
        Assert.True(handler.CanReadToken(token));
    }
}
