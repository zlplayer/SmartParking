using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetAllParkingZones;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.ParkingZone;

[TestSubject(typeof(GetAllParkingZonesQueryHandler))]
public class GetAllParkingZonesQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingZoneRepositories> _parkingZoneRepositoriesMock;
    private readonly GetAllParkingZonesQueryHandler _handler;

    public GetAllParkingZonesQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingZoneRepositoriesMock = new Mock<IParkingZoneRepositories>();
        _handler = new GetAllParkingZonesQueryHandler(
            _mapperMock.Object,
            _parkingZoneRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllParkingZones()
    {
        // Arrange
        var query = new GetAllParkingZonesQuery();

        var parkingZones = new List<Domain.Entities.ParkingZone>
        {
            new() { Id = Guid.NewGuid(), Name = "Zone A", City = "Warsaw" },
            new() { Id = Guid.NewGuid(), Name = "Zone B", City = "Krakow" }
        };

        var expectedDtos = new List<GetParkingZoneDto>
        {
            new() { Id = parkingZones[0].Id, Name = "Zone A", City = "Warsaw" },
            new() { Id = parkingZones[1].Id, Name = "Zone B", City = "Krakow" }
        };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetAllParkingzonesAsync())
            .ReturnsAsync(parkingZones);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingZoneDto>>(parkingZones))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_WhenNoParkingZones_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllParkingZonesQuery();

        var emptyList = new List<Domain.Entities.ParkingZone>();

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetAllParkingzonesAsync())
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingZoneDto>>(emptyList))
            .Returns(new List<GetParkingZoneDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldCallRepository()
    {
        // Arrange
        var query = new GetAllParkingZonesQuery();

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetAllParkingzonesAsync())
            .ReturnsAsync(new List<Domain.Entities.ParkingZone>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingZoneDto>>(It.IsAny<IEnumerable<Domain.Entities.ParkingZone>>()))
            .Returns(new List<GetParkingZoneDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _parkingZoneRepositoriesMock.Verify(x => x.GetAllParkingzonesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapEntitiesToDtos()
    {
        // Arrange
        var query = new GetAllParkingZonesQuery();

        var parkingZones = new List<Domain.Entities.ParkingZone>
        {
            new() { Id = Guid.NewGuid(), Name = "Zone A" }
        };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetAllParkingzonesAsync())
            .ReturnsAsync(parkingZones);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingZoneDto>>(parkingZones))
            .Returns(new List<GetParkingZoneDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<IEnumerable<GetParkingZoneDto>>(parkingZones), Times.Once);
    }
}
