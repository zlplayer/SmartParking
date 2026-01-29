using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CreateParkingSpace;
using SmartParking.Application.SmartParking.Commands.DeleteParkingSpace;
using SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetAllAvailableParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetAllParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetParkingSpace;
using SmartParking.Server.Controllers;
using Xunit;

namespace SmartParking.Server.Tests.Controllers;

[TestSubject(typeof(ParkingSpaceController))]
public class ParkingSpaceControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ParkingSpaceController _controller;

    public ParkingSpaceControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ParkingSpaceController(_mediatorMock.Object);
    }

    #region GetAllParkingSpace

    [Fact]
    public async Task GetAllParkingSpace_ShouldReturnOk_WithParkingSpaces()
    {
        // Arrange
        var parkingSpaces = new List<GetParkingSpaceDto>
        {
            new() { Id = Guid.NewGuid(), Code = "A1", Status = "Available" },
            new() { Id = Guid.NewGuid(), Code = "A2", Status = "Reserved" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parkingSpaces);

        // Act
        var result = await _controller.GetAllParkingSpace();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSpaces = Assert.IsAssignableFrom<IEnumerable<GetParkingSpaceDto>>(okResult.Value);
        Assert.Equal(2, returnedSpaces.Count());
    }

    [Fact]
    public async Task GetAllParkingSpace_ShouldReturnEmptyList_WhenNoSpaces()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetParkingSpaceDto>());

        // Act
        var result = await _controller.GetAllParkingSpace();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSpaces = Assert.IsAssignableFrom<IEnumerable<GetParkingSpaceDto>>(okResult.Value);
        Assert.Empty(returnedSpaces);
    }

    [Fact]
    public async Task GetAllParkingSpace_ShouldSendQueryToMediator()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetParkingSpaceDto>());

        // Act
        await _controller.GetAllParkingSpace();

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllParkingSpaceQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetAllAvailableParkingSpace

    [Fact]
    public async Task GetAllAvailableParkingSpace_ShouldReturnOk_WithAvailableSpaces()
    {
        // Arrange
        var availableSpaces = new List<GetParkingSpaceDto>
        {
            new() { Id = Guid.NewGuid(), Code = "B1", Status = "Available" },
            new() { Id = Guid.NewGuid(), Code = "B2", Status = "Available" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllAvailableParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(availableSpaces);

        // Act
        var result = await _controller.GetAllAvailableParkingSpace();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSpaces = Assert.IsAssignableFrom<IEnumerable<GetParkingSpaceDto>>(okResult.Value);
        Assert.Equal(2, returnedSpaces.Count());
        Assert.All(returnedSpaces, s => Assert.Equal("Available", s.Status));
    }

    [Fact]
    public async Task GetAllAvailableParkingSpace_ShouldReturnEmptyList_WhenNoAvailableSpaces()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllAvailableParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetParkingSpaceDto>());

        // Act
        var result = await _controller.GetAllAvailableParkingSpace();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSpaces = Assert.IsAssignableFrom<IEnumerable<GetParkingSpaceDto>>(okResult.Value);
        Assert.Empty(returnedSpaces);
    }

    [Fact]
    public async Task GetAllAvailableParkingSpace_ShouldSendQueryToMediator()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllAvailableParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetParkingSpaceDto>());

        // Act
        await _controller.GetAllAvailableParkingSpace();

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllAvailableParkingSpaceQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetParkingSpace

    [Fact]
    public async Task GetParkingSpace_ShouldReturnOk_WithParkingSpace()
    {
        // Arrange
        var spaceId = Guid.NewGuid();
        var parkingSpace = new GetParkingSpaceDto
        {
            Id = spaceId,
            Code = "C1",
            Status = "Available"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetParkingSpaceQuery>(q => q.Id == spaceId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parkingSpace);

        // Act
        var result = await _controller.GetParkingSpace(spaceId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSpace = Assert.IsType<GetParkingSpaceDto>(okResult.Value);
        Assert.Equal(spaceId, returnedSpace.Id);
    }

    [Fact]
    public async Task GetParkingSpace_ShouldSendQueryWithCorrectId()
    {
        // Arrange
        var spaceId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetParkingSpaceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetParkingSpaceDto());

        // Act
        await _controller.GetParkingSpace(spaceId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetParkingSpaceQuery>(q => q.Id == spaceId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region CreateParkingSpace

    [Fact]
    public async Task CreateParkingSpace_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateParkingSpaceCommand
        {
            Code = "D1",
            Status = "Available",
            Width = 2.5f,
            Height = 5.0f,
            ParkingZoneId = Guid.NewGuid()
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CreateParkingSpace(command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateParkingSpace_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new CreateParkingSpaceCommand
        {
            Code = "E1",
            ParkingZoneId = Guid.NewGuid()
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.CreateParkingSpace(command);

        // Assert
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateParkingSpace

    [Fact]
    public async Task UpdateParkingSpace_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var spaceId = Guid.NewGuid();
        var command = new UpdateParkingSpaceCommand
        {
            Code = "Updated Code",
            Status = "Reserved"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateParkingSpaceCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateParkingSpace(spaceId, command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateParkingSpace_ShouldSetIdFromRoute()
    {
        // Arrange
        var spaceId = Guid.NewGuid();
        var command = new UpdateParkingSpaceCommand
        {
            Code = "Code"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateParkingSpaceCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.UpdateParkingSpace(spaceId, command);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateParkingSpaceCommand>(c => c.Id == spaceId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region DeleteParkingSpace

    [Fact]
    public async Task DeleteParkingSpace_ShouldReturnOk_WhenSpaceExists()
    {
        // Arrange
        var spaceId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteParkingSpaceCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.DeleteParkingSpace(spaceId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteParkingSpace_ShouldSendCommandWithCorrectId()
    {
        // Arrange
        var spaceId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteParkingSpaceCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.DeleteParkingSpace(spaceId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<DeleteParkingSpaceCommand>(c => c.Id == spaceId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
