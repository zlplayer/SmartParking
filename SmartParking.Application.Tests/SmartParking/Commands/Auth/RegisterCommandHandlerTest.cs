using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Auth;

[TestSubject(typeof(RegisterCommandHandler))]
public class RegisterCommandHandlerTest
{
    private readonly Mock<IUserRepositories> _userRepositoriesMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTest()
    {
        _userRepositoriesMock = new Mock<IUserRepositories>();
        _mapperMock = new Mock<IMapper>();
        _handler = new RegisterCommandHandler(
            _userRepositoriesMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldRegisterUser()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "newuser@example.com",
            Username = "newuser",
            Password = "SecurePassword123",
            FirstName = "New",
            LastName = "User"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Username = command.Username
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByEmail(command.Email))
            .ReturnsAsync((User?)null);

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(command.Username))
            .ReturnsAsync((User?)null);

        _mapperMock
            .Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
            .Returns(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoriesMock.Verify(x => x.CreateUser(user), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "existing@example.com",
            Username = "newuser",
            Password = "SecurePassword123"
        };

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Username = "existinguser"
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByEmail(command.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Użytkownik z takim emailem już istnieje", exception.Message);
    }

    [Fact]
    public async Task Handle_WithExistingUsername_ShouldThrowException()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "newuser@example.com",
            Username = "existinguser",
            Password = "SecurePassword123"
        };

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "other@example.com",
            Username = command.Username
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByEmail(command.Email))
            .ReturnsAsync((User?)null);

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(command.Username))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal("Użytkownik z taką nazwą już istnieje", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldHashPasswordBeforeMapping()
    {
        // Arrange
        var originalPassword = "SecurePassword123";
        var command = new RegisterCommand
        {
            Email = "newuser@example.com",
            Username = "newuser",
            Password = originalPassword
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByEmail(command.Email))
            .ReturnsAsync((User?)null);

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(command.Username))
            .ReturnsAsync((User?)null);

        _mapperMock
            .Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
            .Returns(new User());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        // Password should have been modified to a BCrypt hash
        Assert.NotEqual(originalPassword, command.Password);
        Assert.True(BCrypt.Net.BCrypt.Verify(originalPassword, command.Password));
    }

    [Fact]
    public async Task Handle_ShouldCheckEmailBeforeUsername()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "existing@example.com",
            Username = "newuser",
            Password = "SecurePassword123"
        };

        var existingUser = new User { Email = command.Email };

        _userRepositoriesMock
            .Setup(x => x.GetUserByEmail(command.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));

        // Username check should not be called if email already exists
        _userRepositoriesMock.Verify(x => x.GetUserByUsername(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToUserEntity()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "newuser@example.com",
            Username = "newuser",
            Password = "SecurePassword123",
            FirstName = "Test",
            LastName = "User"
        };

        _userRepositoriesMock
            .Setup(x => x.GetUserByEmail(command.Email))
            .ReturnsAsync((User?)null);

        _userRepositoriesMock
            .Setup(x => x.GetUserByUsername(command.Username))
            .ReturnsAsync((User?)null);

        _mapperMock
            .Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
            .Returns(new User());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<User>(It.IsAny<RegisterCommand>()), Times.Once);
    }
}
