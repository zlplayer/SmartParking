using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.DeleteParkingZone;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.ParkingZone;

[TestSubject(typeof(DeleteParkingZoneCommandHandler))]
public class DeleteParkingZoneCommandHandlerTest
{
    private readonly Mock<IParkingZoneRepositories> _parkingZoneRepositoriesMock;
    private readonly DeleteParkingZoneCommandHandler _handler;

    public DeleteParkingZoneCommandHandlerTest()
    {
        _parkingZoneRepositoriesMock = new Mock<IParkingZoneRepositories>();
        _handler = new DeleteParkingZoneCommandHandler(_parkingZoneRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneExists_ShouldDeleteParkingZone()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var command = new DeleteParkingZoneCommand { Id = parkingZoneId };

        var existingParkingZone = new Domain.Entities.ParkingZone
        {
            Id = parkingZoneId,
            Name = "Zone to delete"
        };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(existingParkingZone);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingZoneRepositoriesMock.Verify(x => x.DeleteParkingZoneAsync(existingParkingZone), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteParkingZoneCommand { Id = Guid.NewGuid() };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(command.Id))
            .ReturnsAsync((Domain.Entities.ParkingZone?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallGetParkingZoneWithCorrectId()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var command = new DeleteParkingZoneCommand { Id = parkingZoneId };

        var existingParkingZone = new Domain.Entities.ParkingZone { Id = parkingZoneId };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(existingParkingZone);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingZoneRepositoriesMock.Verify(x => x.GetParkingZoneAsync(parkingZoneId), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneNotFound_ShouldNotCallDeleteMethod()
    {
        // Arrange
        var command = new DeleteParkingZoneCommand { Id = Guid.NewGuid() };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(command.Id))
            .ReturnsAsync((Domain.Entities.ParkingZone?)null);

        // Act & Assert
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (KeyNotFoundException)
        {
            // Expected
        }

        _parkingZoneRepositoriesMock.Verify(
            x => x.DeleteParkingZoneAsync(It.IsAny<Domain.Entities.ParkingZone>()),
            Times.Never);
    }
}
