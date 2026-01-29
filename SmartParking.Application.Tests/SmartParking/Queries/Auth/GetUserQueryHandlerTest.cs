using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetUser;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.Auth;

[TestSubject(typeof(GetUserQueryHandler))]
public class GetUserQueryHandlerTest
{
    private readonly Mock<IUserRepositories> _userRepositoriesMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserQueryHandler _handler;

    public GetUserQueryHandlerTest()
    {
        _userRepositoriesMock = new Mock<IUserRepositories>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserQueryHandler(
            _userRepositoriesMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldReturnUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserQuery { UserId = userId };

        var user = new User
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        var expectedDto = new UserDto
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            RoleName = "User"
        };

        _userRepositoriesMock
            .Setup(x => x.GetUser(userId))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Username, result.Username);
        Assert.Equal(expectedDto.Email, result.Email);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldThrowException()
    {
        // Arrange
        var query = new GetUserQuery { UserId = Guid.NewGuid() };

        _userRepositoriesMock
            .Setup(x => x.GetUser(query.UserId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(query, CancellationToken.None));

        Assert.Equal("Użytkownik nie znaleziony", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserQuery { UserId = userId };

        var user = new User { Id = userId };

        _userRepositoriesMock
            .Setup(x => x.GetUser(userId))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(new UserDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _userRepositoriesMock.Verify(x => x.GetUser(userId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapUserToUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserQuery { UserId = userId };

        var user = new User { Id = userId };

        _userRepositoriesMock
            .Setup(x => x.GetUser(userId))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(new UserDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<UserDto>(user), Times.Once);
    }
}
