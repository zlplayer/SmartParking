using System.Security.Claims;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetUser;
using SmartParking.Server.Controllers;
using Xunit;

namespace SmartParking.Server.Tests.Controllers;

[TestSubject(typeof(UserController))]
public class UserControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UserController _controller;

    public UserControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);
    }

    private void SetupUserClaims(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    #region GetUser

    [Fact]
    public async Task GetUser_ShouldReturnOk_WithUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new UserDto
        {
            Id = userId,
            Username = "testuser",
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetUserQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
        Assert.Equal("testuser", returnedUser.Username);
    }

    [Fact]
    public async Task GetUser_ShouldSendQueryToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserDto());

        // Act
        await _controller.GetUser(userId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetUserQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetUser_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto?)null);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
    }

    #endregion

    #region GetCurrentUser

    [Fact]
    public async Task GetCurrentUser_ShouldReturnOk_WithCurrentUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetupUserClaims(userId);

        var expectedUser = new UserDto
        {
            Id = userId,
            Username = "currentuser",
            Email = "current@test.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetUserQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
    }

    [Fact]
    public async Task GetCurrentUser_ShouldUseUserIdFromClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetupUserClaims(userId);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserDto { Id = userId });

        // Act
        await _controller.GetCurrentUser();

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetUserQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
