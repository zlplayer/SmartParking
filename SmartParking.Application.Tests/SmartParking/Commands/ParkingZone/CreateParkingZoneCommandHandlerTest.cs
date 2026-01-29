using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.CreateParkingZone;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.ParkingZone;

[TestSubject(typeof(CreateParkingZoneCommandHandler))]
public class CreateParkingZoneCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingZoneRepositories> _parkingZoneRepositoriesMock;
    private readonly CreateParkingZoneCommandHandler _handler;

    public CreateParkingZoneCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingZoneRepositoriesMock = new Mock<IParkingZoneRepositories>();
        _handler = new CreateParkingZoneCommandHandler(
            _mapperMock.Object,
            _parkingZoneRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateParkingZone()
    {
        // Arrange
        var command = new CreateParkingZoneCommand
        {
            Name = "Zone A",
            City = "Warsaw",
            TariffType = "Standard",
            Regulamin = "Rules apply",
            IsActive = true,
            UserId = Guid.NewGuid()
        };

        var parkingZone = new Domain.Entities.ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            City = command.City
        };

        _mapperMock
            .Setup(x => x.Map<CreateParkingZoneCommand, Domain.Entities.ParkingZone>(command))
            .Returns(parkingZone);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingZoneRepositoriesMock.Verify(x => x.CreateParkingZoneAsync(parkingZone), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToEntity()
    {
        // Arrange
        var command = new CreateParkingZoneCommand
        {
            Name = "Zone B",
            City = "Krakow",
            TariffType = "Premium",
            Regulamin = "Premium rules",
            IsActive = true
        };

        _mapperMock
            .Setup(x => x.Map<CreateParkingZoneCommand, Domain.Entities.ParkingZone>(command))
            .Returns(new Domain.Entities.ParkingZone());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<CreateParkingZoneCommand, Domain.Entities.ParkingZone>(command), Times.Once);
    }
}
