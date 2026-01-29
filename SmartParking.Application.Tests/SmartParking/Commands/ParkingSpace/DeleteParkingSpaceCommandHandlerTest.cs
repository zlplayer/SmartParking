using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.DeleteParkingSpace;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.ParkingSpace;

[TestSubject(typeof(DeleteParkingSpaceCommandHandler))]
public class DeleteParkingSpaceCommandHandlerTest
{
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly DeleteParkingSpaceCommandHandler _handler;

    public DeleteParkingSpaceCommandHandlerTest()
    {
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new DeleteParkingSpaceCommandHandler(_parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceExists_ShouldDeleteParkingSpace()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new DeleteParkingSpaceCommand { Id = parkingSpaceId };

        var existingParkingSpace = new Domain.Entities.ParkingSpace
        {
            Id = parkingSpaceId,
            Code = "A1"
        };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(existingParkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.DeleteParkingSpaceAsync(existingParkingSpace), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteParkingSpaceCommand { Id = Guid.NewGuid() };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallGetParkingSpaceWithCorrectId()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new DeleteParkingSpaceCommand { Id = parkingSpaceId };

        var existingParkingSpace = new Domain.Entities.ParkingSpace { Id = parkingSpaceId };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(existingParkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.GetParkingSpaceByIdAsync(parkingSpaceId), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceNotFound_ShouldNotCallDeleteMethod()
    {
        // Arrange
        var command = new DeleteParkingSpaceCommand { Id = Guid.NewGuid() };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act & Assert
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (KeyNotFoundException)
        {
            // Expected
        }

        _parkingSpaceRepositoriesMock.Verify(
            x => x.DeleteParkingSpaceAsync(It.IsAny<Domain.Entities.ParkingSpace>()),
            Times.Never);
    }
}
