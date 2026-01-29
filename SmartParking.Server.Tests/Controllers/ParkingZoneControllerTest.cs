using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CreateParkingZone;
using SmartParking.Application.SmartParking.Commands.DeleteParkingZone;
using SmartParking.Application.SmartParking.Commands.UpdateParkingZone;
using SmartParking.Application.SmartParking.Queries.GetAllParkingZones;
using SmartParking.Application.SmartParking.Queries.GetParkingZone;
using SmartParking.Server.Controllers;
using Xunit;

namespace SmartParking.Server.Tests.Controllers;

[TestSubject(typeof(ParkingZoneController))]
public class ParkingZoneControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ParkingZoneController _controller;

    public ParkingZoneControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ParkingZoneController(_mediatorMock.Object);
    }

    #region GetAllParkingZone

    [Fact]
    public async Task GetAllParkingZone_ShouldReturnOk_WithParkingZones()
    {
        // Arrange
        var parkingZones = new List<GetParkingZoneDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Zone A", City = "Warsaw" },
            new() { Id = Guid.NewGuid(), Name = "Zone B", City = "Krakow" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllParkingZonesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parkingZones);

        // Act
        var result = await _controller.GetAllParkingZone();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedZones = Assert.IsAssignableFrom<IEnumerable<GetParkingZoneDto>>(okResult.Value);
        Assert.Equal(2, returnedZones.Count());
    }

    [Fact]
    public async Task GetAllParkingZone_ShouldReturnEmptyList_WhenNoZones()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllParkingZonesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetParkingZoneDto>());

        // Act
        var result = await _controller.GetAllParkingZone();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedZones = Assert.IsAssignableFrom<IEnumerable<GetParkingZoneDto>>(okResult.Value);
        Assert.Empty(returnedZones);
    }

    [Fact]
    public async Task GetAllParkingZone_ShouldSendQueryToMediator()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllParkingZonesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetParkingZoneDto>());

        // Act
        await _controller.GetAllParkingZone();

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllParkingZonesQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetParkingZone

    [Fact]
    public async Task GetParkingZone_ShouldReturnOk_WithParkingZone()
    {
        // Arrange
        var zoneId = Guid.NewGuid();
        var parkingZone = new GetParkingZoneDto
        {
            Id = zoneId,
            Name = "Test Zone",
            City = "Test City"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetParkingZoneQuery>(q => q.Id == zoneId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parkingZone);

        // Act
        var result = await _controller.GetParkingZone(zoneId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedZone = Assert.IsType<GetParkingZoneDto>(okResult.Value);
        Assert.Equal(zoneId, returnedZone.Id);
    }

    [Fact]
    public async Task GetParkingZone_ShouldSendQueryWithCorrectId()
    {
        // Arrange
        var zoneId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetParkingZoneQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetParkingZoneDto());

        // Act
        await _controller.GetParkingZone(zoneId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetParkingZoneQuery>(q => q.Id == zoneId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region CreateParkingZone

    [Fact]
    public async Task CreateParkingZone_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateParkingZoneCommand
        {
            Name = "New Zone",
            City = "New City"
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CreateParkingZone(command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateParkingZone_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new CreateParkingZoneCommand
        {
            Name = "Zone",
            City = "City"
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.CreateParkingZone(command);

        // Assert
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateParkingZone

    [Fact]
    public async Task UpdateParkingZone_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var zoneId = Guid.NewGuid();
        var command = new UpdateParkingZoneCommand
        {
            Name = "Updated Zone",
            City = "Updated City"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateParkingZoneCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateParkingZone(zoneId, command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateParkingZone_ShouldSetIdFromRoute()
    {
        // Arrange
        var zoneId = Guid.NewGuid();
        var command = new UpdateParkingZoneCommand
        {
            Name = "Zone",
            City = "City"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateParkingZoneCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.UpdateParkingZone(zoneId, command);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateParkingZoneCommand>(c => c.Id == zoneId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region DeleteParkingZone

    [Fact]
    public async Task DeleteParkingZone_ShouldReturnOk_WhenZoneExists()
    {
        // Arrange
        var zoneId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteParkingZoneCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.DeleteParkingZone(zoneId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteParkingZone_ShouldSendCommandWithCorrectId()
    {
        // Arrange
        var zoneId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteParkingZoneCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.DeleteParkingZone(zoneId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<DeleteParkingZoneCommand>(c => c.Id == zoneId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
