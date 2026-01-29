using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.UpdateParkingZone;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.ParkingZone;

[TestSubject(typeof(UpdateParkingZoneCommandHandler))]
public class UpdateParkingZoneCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingZoneRepositories> _parkingZoneRepositoriesMock;
    private readonly UpdateParkingZoneCommandHandler _handler;

    public UpdateParkingZoneCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingZoneRepositoriesMock = new Mock<IParkingZoneRepositories>();
        _handler = new UpdateParkingZoneCommandHandler(
            _mapperMock.Object,
            _parkingZoneRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneExists_ShouldUpdateParkingZone()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var command = new UpdateParkingZoneCommand
        {
            Id = parkingZoneId,
            Name = "Updated Zone",
            City = "Updated City",
            TariffType = "Premium",
            Regulamin = "Updated rules",
            IsActive = true
        };

        var existingParkingZone = new Domain.Entities.ParkingZone
        {
            Id = parkingZoneId,
            Name = "Original Zone",
            City = "Original City"
        };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(existingParkingZone);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, existingParkingZone), Times.Once);
        _parkingZoneRepositoriesMock.Verify(x => x.UpdateParkingZoneAsync(existingParkingZone), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateParkingZoneCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Zone"
        };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(command.Id))
            .ReturnsAsync((Domain.Entities.ParkingZone?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var command = new UpdateParkingZoneCommand
        {
            Id = parkingZoneId,
            Name = "Test Zone"
        };

        var existingParkingZone = new Domain.Entities.ParkingZone { Id = parkingZoneId };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(existingParkingZone);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingZoneRepositoriesMock.Verify(x => x.GetParkingZoneAsync(parkingZoneId), Times.Once);
    }
}
