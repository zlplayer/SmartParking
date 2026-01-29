using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetParkingZone;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.ParkingZone;

[TestSubject(typeof(GetParkingZoneQueryHandler))]
public class GetParkingZoneQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingZoneRepositories> _parkingZoneRepositoriesMock;
    private readonly GetParkingZoneQueryHandler _handler;

    public GetParkingZoneQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingZoneRepositoriesMock = new Mock<IParkingZoneRepositories>();
        _handler = new GetParkingZoneQueryHandler(
            _mapperMock.Object,
            _parkingZoneRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneExists_ShouldReturnDto()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var query = new GetParkingZoneQuery { Id = parkingZoneId };

        var parkingZone = new Domain.Entities.ParkingZone
        {
            Id = parkingZoneId,
            Name = "Zone A",
            City = "Warsaw"
        };

        var expectedDto = new GetParkingZoneDto
        {
            Id = parkingZoneId,
            Name = "Zone A",
            City = "Warsaw"
        };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(parkingZone);

        _mapperMock
            .Setup(x => x.Map<GetParkingZoneDto>(parkingZone))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Name, result.Name);
    }

    [Fact]
    public async Task Handle_WhenParkingZoneNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var query = new GetParkingZoneQuery { Id = Guid.NewGuid() };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(query.Id))
            .ReturnsAsync((Domain.Entities.ParkingZone?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var query = new GetParkingZoneQuery { Id = parkingZoneId };

        var parkingZone = new Domain.Entities.ParkingZone { Id = parkingZoneId };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(parkingZone);

        _mapperMock
            .Setup(x => x.Map<GetParkingZoneDto>(parkingZone))
            .Returns(new GetParkingZoneDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _parkingZoneRepositoriesMock.Verify(x => x.GetParkingZoneAsync(parkingZoneId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapEntityToDto()
    {
        // Arrange
        var parkingZoneId = Guid.NewGuid();
        var query = new GetParkingZoneQuery { Id = parkingZoneId };

        var parkingZone = new Domain.Entities.ParkingZone { Id = parkingZoneId };

        _parkingZoneRepositoriesMock
            .Setup(x => x.GetParkingZoneAsync(parkingZoneId))
            .ReturnsAsync(parkingZone);

        _mapperMock
            .Setup(x => x.Map<GetParkingZoneDto>(parkingZone))
            .Returns(new GetParkingZoneDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<GetParkingZoneDto>(parkingZone), Times.Once);
    }
}
