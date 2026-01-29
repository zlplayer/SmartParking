using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CreateVehicle;
using SmartParking.Application.SmartParking.Commands.DeleteVehicle;
using SmartParking.Application.SmartParking.Commands.UpdateVehicle;
using SmartParking.Application.SmartParking.Queries.GetAllVehicleByUserId;
using SmartParking.Server.Controllers;
using Xunit;

namespace SmartParking.Server.Tests.Controllers;

[TestSubject(typeof(VehicleController))]
public class VehicleControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly VehicleController _controller;

    public VehicleControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new VehicleController(_mediatorMock.Object);
    }

    #region GetAllVehiclesByUserId

    [Fact]
    public async Task GetAllVehiclesByUserId_ShouldReturnOk_WithVehicles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var vehicles = new List<GetVehicleDto>
        {
            new() { Id = Guid.NewGuid(), Brand = "Toyota", Model = "Corolla" },
            new() { Id = Guid.NewGuid(), Brand = "Honda", Model = "Civic" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetAllVehicleByUserIdQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicles);

        // Act
        var result = await _controller.GetAllVehiclesByUserId(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedVehicles = Assert.IsAssignableFrom<IEnumerable<GetVehicleDto>>(okResult.Value);
        Assert.Equal(2, returnedVehicles.Count());
    }

    [Fact]
    public async Task GetAllVehiclesByUserId_ShouldReturnEmptyList_WhenNoVehicles()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllVehicleByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetVehicleDto>());

        // Act
        var result = await _controller.GetAllVehiclesByUserId(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedVehicles = Assert.IsAssignableFrom<IEnumerable<GetVehicleDto>>(okResult.Value);
        Assert.Empty(returnedVehicles);
    }

    [Fact]
    public async Task GetAllVehiclesByUserId_ShouldSendQueryToMediator()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllVehicleByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetVehicleDto>());

        // Act
        await _controller.GetAllVehiclesByUserId(userId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetAllVehicleByUserIdQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region CreateVehicle

    [Fact]
    public async Task CreateVehicle_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateVehicleCommand
        {
            Brand = "Toyota",
            Model = "Camry",
            RegistrationNumber = "WA12345",
            UserId = Guid.NewGuid()
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CreateVehicle(command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateVehicle_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new CreateVehicleCommand
        {
            Brand = "Honda",
            Model = "Accord",
            RegistrationNumber = "WA54321",
            UserId = Guid.NewGuid()
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.CreateVehicle(command);

        // Assert
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateVehicle

    [Fact]
    public async Task UpdateVehicle_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new UpdateVehicleCommand
        {
            Brand = "Updated Brand",
            Model = "Updated Model"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateVehicleCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.UpdateVehicle(vehicleId, command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateVehicle_ShouldSetVehicleIdFromRoute()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new UpdateVehicleCommand
        {
            Brand = "Brand",
            Model = "Model"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateVehicleCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.UpdateVehicle(vehicleId, command);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateVehicleCommand>(c => c.Id == vehicleId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region DeleteVehicle

    [Fact]
    public async Task DeleteVehicle_ShouldReturnOk_WhenVehicleExists()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteVehicleCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.DeleteVehicle(vehicleId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteVehicle_ShouldSendCommandWithCorrectId()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteVehicleCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.DeleteVehicle(vehicleId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<DeleteVehicleCommand>(c => c.Id == vehicleId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
