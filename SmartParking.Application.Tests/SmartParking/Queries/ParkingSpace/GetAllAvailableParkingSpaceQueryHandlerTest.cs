using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetAllAvailableParkingSpace;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.ParkingSpace;

[TestSubject(typeof(GetAllAvailableParkingSpaceQueryHandler))]
public class GetAllAvailableParkingSpaceQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly GetAllAvailableParkingSpaceQueryHandler _handler;

    public GetAllAvailableParkingSpaceQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new GetAllAvailableParkingSpaceQueryHandler(
            _mapperMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOnlyAvailableParkingSpaces()
    {
        // Arrange
        var query = new GetAllAvailableParkingSpaceQuery();

        var availableParkingSpaces = new List<Domain.Entities.ParkingSpace>
        {
            new() { Id = Guid.NewGuid(), Code = "A1", Status = "Available" },
            new() { Id = Guid.NewGuid(), Code = "A2", Status = "Available" }
        };

        var expectedDtos = new List<GetParkingSpaceDto>
        {
            new() { Id = availableParkingSpaces[0].Id, Code = "A1", Status = "Available" },
            new() { Id = availableParkingSpaces[1].Id, Code = "A2", Status = "Available" }
        };

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetAllAvailableParkingSpaceAsync())
            .ReturnsAsync(availableParkingSpaces);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingSpaceDto>>(availableParkingSpaces))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, dto => Assert.Equal("Available", dto.Status));
    }

    [Fact]
    public async Task Handle_WhenNoAvailableParkingSpaces_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllAvailableParkingSpaceQuery();

        var emptyList = new List<Domain.Entities.ParkingSpace>();

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetAllAvailableParkingSpaceAsync())
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
    public async Task Handle_ShouldCallGetAllAvailableParkingSpaceAsyncRepository()
    {
        // Arrange
        var query = new GetAllAvailableParkingSpaceQuery();

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetAllAvailableParkingSpaceAsync())
            .ReturnsAsync(new List<Domain.Entities.ParkingSpace>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetParkingSpaceDto>>(It.IsAny<IEnumerable<Domain.Entities.ParkingSpace>>()))
            .Returns(new List<GetParkingSpaceDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.GetAllAvailableParkingSpaceAsync(), Times.Once);
    }
}
