using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.DeleteVehicle;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Vehicle;

[TestSubject(typeof(DeleteVehicleCommandHandler))]
public class DeleteVehicleCommandHandlerTest
{
    private readonly Mock<IVehicleRepositories> _vehicleRepositoriesMock;
    private readonly DeleteVehicleCommandHandler _handler;

    public DeleteVehicleCommandHandlerTest()
    {
        _vehicleRepositoriesMock = new Mock<IVehicleRepositories>();
        _handler = new DeleteVehicleCommandHandler(_vehicleRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenVehicleExists_ShouldDeleteVehicle()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new DeleteVehicleCommand { Id = vehicleId };

        var existingVehicle = new Domain.Entities.Vehicle
        {
            Id = vehicleId,
            Brand = "Toyota",
            Model = "Camry"
        };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(existingVehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _vehicleRepositoriesMock.Verify(x => x.DeleteVehicleAsync(existingVehicle), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteVehicleCommand { Id = Guid.NewGuid() };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Vehicle?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallGetVehicleWithCorrectId()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new DeleteVehicleCommand { Id = vehicleId };

        var existingVehicle = new Domain.Entities.Vehicle { Id = vehicleId };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(existingVehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _vehicleRepositoriesMock.Verify(x => x.GetVehicleAsync(vehicleId), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldNotCallDeleteMethod()
    {
        // Arrange
        var command = new DeleteVehicleCommand { Id = Guid.NewGuid() };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Vehicle?)null);

        // Act & Assert
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (KeyNotFoundException)
        {
            // Expected
        }

        _vehicleRepositoriesMock.Verify(
            x => x.DeleteVehicleAsync(It.IsAny<Domain.Entities.Vehicle>()),
            Times.Never);
    }
}
