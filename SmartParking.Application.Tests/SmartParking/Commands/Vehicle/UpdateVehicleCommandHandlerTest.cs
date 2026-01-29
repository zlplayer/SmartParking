using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.UpdateVehicle;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Vehicle;

[TestSubject(typeof(UpdateVehicleCommandHandler))]
public class UpdateVehicleCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IVehicleRepositories> _vehicleRepositoriesMock;
    private readonly UpdateVehicleCommandHandler _handler;

    public UpdateVehicleCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _vehicleRepositoriesMock = new Mock<IVehicleRepositories>();
        _handler = new UpdateVehicleCommandHandler(
            _mapperMock.Object,
            _vehicleRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenVehicleExists_ShouldUpdateVehicle()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new UpdateVehicleCommand
        {
            Id = vehicleId,
            Brand = "Updated Brand",
            Model = "Updated Model",
            RegistrationNumber = "WA99999",
            CountryCode = "PL"
        };

        var existingVehicle = new Domain.Entities.Vehicle
        {
            Id = vehicleId,
            Brand = "Original Brand",
            Model = "Original Model"
        };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(existingVehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, existingVehicle), Times.Once);
        _vehicleRepositoriesMock.Verify(x => x.UpdateVehicleAsync(existingVehicle), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateVehicleCommand
        {
            Id = Guid.NewGuid(),
            Brand = "Updated Brand"
        };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Vehicle?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new UpdateVehicleCommand
        {
            Id = vehicleId,
            Brand = "Test Brand"
        };

        var existingVehicle = new Domain.Entities.Vehicle { Id = vehicleId };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(existingVehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _vehicleRepositoriesMock.Verify(x => x.GetVehicleAsync(vehicleId), Times.Once);
    }
}
