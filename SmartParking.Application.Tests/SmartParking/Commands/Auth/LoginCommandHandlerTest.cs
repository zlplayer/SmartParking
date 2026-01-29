using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.Interfaces;
using SmartParking.Application.SmartParking.Commands.LoginCommand;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Auth;

[TestSubject(typeof(LoginCommandHandler))]
public class LoginCommandHandlerTest
{
    private readonly Mock<IUserRepositories> _userRepositoriesMock;
    private readonly Mock<IJwtAuth> _jwtAuthMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTest()
    {
        _userRepositoriesMock = new Mock<IUserRepositories>();
        _jwtAuthMock = new Mock<IJwtAuth>();
        _mapperMock = new Mock<IMapper>();
        _handler = new LoginCommandHandler(
            _userRepositoriesMock.Object,
            _jwtAuthMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnAuthResponseWithToken()
    {
        // Arrange
        var username = "testuser";
        var password = "TestPassword123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var userId = Guid.NewGuid();

        var command = new LoginCommand
        {
            Username = username,
            Password = password
        };

        var user = new User
        {
            Id = userId,
            Username = username,
            Email = "test@example.com",
            PasswordHash = hashedPassword,
            FirstName = "Test",
            LastName = "User"
        };

        var userDto = new UserDto
        {
            Id = userId,
            Username = username,
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            RoleName = "User"
        };

        var expectedToken = "generated.jwt.token";

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(username))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(userDto);

        _jwtAuthMock
            .Setup(x => x.GenerateToken(userDto))
            .Returns(expectedToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        Assert.Equal(userDto, result.User);
    }

    [Fact]
    public async Task Handle_WithInvalidUsername_ShouldThrowException()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "nonexistent",
            Password = "SomePassword123"
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(command.Username))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Nieprawidłowa nazwa użytkownika lub hasło", exception.Message);
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldThrowException()
    {
        // Arrange
        var username = "testuser";
        var correctPassword = "CorrectPassword123";
        var wrongPassword = "WrongPassword123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

        var command = new LoginCommand
        {
            Username = username,
            Password = wrongPassword
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = "test@example.com",
            PasswordHash = hashedPassword
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(username))
            .ReturnsAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Nieprawidłowa nazwa użytkownika lub hasło", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldCallGenerateTokenWithUserDto()
    {
        // Arrange
        var username = "testuser";
        var password = "TestPassword123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var command = new LoginCommand
        {
            Username = username,
            Password = password
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = "test@example.com",
            PasswordHash = hashedPassword
        };

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = username,
            Email = "test@example.com"
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(username))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(userDto);

        _jwtAuthMock
            .Setup(x => x.GenerateToken(userDto))
            .Returns("token");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _jwtAuthMock.Verify(x => x.GenerateToken(userDto), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapUserToUserDto()
    {
        // Arrange
        var username = "testuser";
        var password = "TestPassword123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var command = new LoginCommand
        {
            Username = username,
            Password = password
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = "test@example.com",
            PasswordHash = hashedPassword
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(username))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(new UserDto());

        _jwtAuthMock
            .Setup(x => x.GenerateToken(It.IsAny<UserDto>()))
            .Returns("token");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<UserDto>(user), Times.Once);
    }
}
