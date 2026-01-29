using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.LoginCommand;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;
using SmartParking.Server.Controllers;
using Xunit;

namespace SmartParking.Server.Tests.Controllers;

[TestSubject(typeof(AuthController))]
public class AuthControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    #region Register

    [Fact]
    public async Task Register_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "Password123!",
            FirstName = "Test",
            LastName = "User"
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.Register(command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Register_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "Password123!",
            FirstName = "Test",
            LastName = "User"
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.Register(command);

        // Assert
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Login

    [Fact]
    public async Task Login_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = "Password123!"
        };
        var expectedResponse = new AuthResponseDto
        {
            Token = "jwt-token-123",
            User = new UserDto { Username = "testuser" }
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Login(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal("jwt-token-123", response.Token);
    }

    [Fact]
    public async Task Login_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = "Password123!"
        };
        var expectedResponse = new AuthResponseDto { Token = "token" };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _controller.Login(command);

        // Assert
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldReturnAuthResponse_FromMediator()
    {
        // Arrange
        var command = new LoginCommand
        {
            Username = "testuser",
            Password = "Password123!"
        };
        var expectedResponse = new AuthResponseDto
        {
            Token = "expected-jwt-token",
            User = new UserDto { Id = Guid.NewGuid(), Username = "testuser" }
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Login(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal("expected-jwt-token", response.Token);
        Assert.NotNull(response.User);
    }

    #endregion
}
