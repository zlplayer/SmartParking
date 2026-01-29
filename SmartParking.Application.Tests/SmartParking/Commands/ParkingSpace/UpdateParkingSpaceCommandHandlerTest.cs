using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.ParkingSpace;

[TestSubject(typeof(UpdateParkingSpaceCommandHandler))]
public class UpdateParkingSpaceCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly UpdateParkingSpaceCommandHandler _handler;

    public UpdateParkingSpaceCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new UpdateParkingSpaceCommandHandler(
            _mapperMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceExists_ShouldUpdateParkingSpace()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new UpdateParkingSpaceCommand
        {
            Id = parkingSpaceId,
            Code = "A1-Updated",
            Width = 3.0f,
            Height = 6.0f,
            Status = "Reserved"
        };

        var existingParkingSpace = new Domain.Entities.ParkingSpace
        {
            Id = parkingSpaceId,
            Code = "A1",
            Width = 2.5f,
            Height = 5.0f,
            Status = "Available"
        };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(existingParkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, existingParkingSpace), Times.Once);
        _parkingSpaceRepositoriesMock.Verify(x => x.UpdateParkingSpaceAsync(existingParkingSpace), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateParkingSpaceCommand
        {
            Id = Guid.NewGuid(),
            Code = "A1"
        };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new UpdateParkingSpaceCommand
        {
            Id = parkingSpaceId,
            Code = "Test"
        };

        var existingParkingSpace = new Domain.Entities.ParkingSpace { Id = parkingSpaceId };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(existingParkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.GetParkingSpaceByIdAsync(parkingSpaceId), Times.Once);
    }
}
