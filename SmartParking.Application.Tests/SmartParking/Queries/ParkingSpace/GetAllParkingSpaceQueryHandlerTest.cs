using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetAllParkingSpace;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.ParkingSpace;

[TestSubject(typeof(GetAllParkingSpaceQueryHandler))]
public class GetAllParkingSpaceQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly GetAllParkingSpaceQueryHandler _handler;

    public GetAllParkingSpaceQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new GetAllParkingSpaceQueryHandler(
            _mapperMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllParkingSpaces()
    {
        // Arrange
        var query = new GetAllParkingSpaceQuery();

        var parkingSpaces = new List<Domain.Entities.ParkingSpace>
        {
            new() { Id = Guid.NewGuid(), Code = "A1", Status = "Available" },
            new() { Id = Guid.NewGuid(), Code = "A2", Status = "Reserved" }
        };

        var expectedDtos = new List<GetParkingSpaceDto>
        {
            new() { Id = parkingSpaces[0].Id, Code = "A1", Status = "Available" },
            new() { Id = parkingSpaces[1].Id, Code = "A2", Status = "Reserved" }
        };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetAllParkingSpaceAsync())
            .ReturnsAsync(parkingSpaces);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingSpaceDto>>(parkingSpaces))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_WhenNoParkingSpaces_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllParkingSpaceQuery();

        var emptyList = new List<Domain.Entities.ParkingSpace>();

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetAllParkingSpaceAsync())
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingSpaceDto>>(emptyList))
            .Returns(new List<GetParkingSpaceDto>());

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
        var query = new GetAllParkingSpaceQuery();

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetAllParkingSpaceAsync())
            .ReturnsAsync(new List<Domain.Entities.ParkingSpace>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingSpaceDto>>(It.IsAny<IEnumerable<Domain.Entities.ParkingSpace>>()))
            .Returns(new List<GetParkingSpaceDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.GetAllParkingSpaceAsync(), Times.Once);
    }
}
