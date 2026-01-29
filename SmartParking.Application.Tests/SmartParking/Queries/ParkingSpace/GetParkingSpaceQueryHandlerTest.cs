using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetParkingSpace;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.ParkingSpace;

[TestSubject(typeof(GetParkingSpaceQueryHandler))]
public class GetParkingSpaceQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly GetParkingSpaceQueryHandler _handler;

    public GetParkingSpaceQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new GetParkingSpaceQueryHandler(
            _mapperMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceExists_ShouldReturnDto()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var query = new GetParkingSpaceQuery { Id = parkingSpaceId };

        var parkingSpace = new Domain.Entities.ParkingSpace
        {
            Id = parkingSpaceId,
            Code = "A1",
            Status = "Available"
        };

        var expectedDto = new GetParkingSpaceDto
        {
            Id = parkingSpaceId,
            Code = "A1",
            Status = "Available"
        };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(parkingSpace);

        _mapperMock
            .Setup(x => x.Map<GetParkingSpaceDto>(parkingSpace))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Code, result.Code);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var query = new GetParkingSpaceQuery { Id = parkingSpaceId };

        var parkingSpace = new Domain.Entities.ParkingSpace { Id = parkingSpaceId };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(parkingSpace);

        _mapperMock
            .Setup(x => x.Map<GetParkingSpaceDto>(parkingSpace))
            .Returns(new GetParkingSpaceDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.GetParkingSpaceByIdAsync(parkingSpaceId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapEntityToDto()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var query = new GetParkingSpaceQuery { Id = parkingSpaceId };

        var parkingSpace = new Domain.Entities.ParkingSpace { Id = parkingSpaceId };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(parkingSpace);

        _mapperMock
            .Setup(x => x.Map<GetParkingSpaceDto>(parkingSpace))
            .Returns(new GetParkingSpaceDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<GetParkingSpaceDto>(parkingSpace), Times.Once);
    }
}
