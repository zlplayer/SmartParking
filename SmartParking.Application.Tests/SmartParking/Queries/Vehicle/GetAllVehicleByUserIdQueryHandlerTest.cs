using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetAllVehicleByUserId;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.Vehicle;

[TestSubject(typeof(GetAllVehicleByUserIdQueryHandler))]
public class GetAllVehicleByUserIdQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IVehicleRepositories> _vehicleRepositoriesMock;
    private readonly GetAllVehicleByUserIdQueryHandler _handler;

    public GetAllVehicleByUserIdQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _vehicleRepositoriesMock = new Mock<IVehicleRepositories>();
        _handler = new GetAllVehicleByUserIdQueryHandler(
            _mapperMock.Object,
            _vehicleRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnVehiclesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetAllVehicleByUserIdQuery { UserId = userId };

        var vehicles = new List<Domain.Entities.Vehicle>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Brand = "Toyota" },
            new() { Id = Guid.NewGuid(), UserId = userId, Brand = "Honda" }
        };

        var expectedDtos = new List<GetVehicleDto>
        {
            new() { Id = vehicles[0].Id, Brand = "Toyota" },
            new() { Id = vehicles[1].Id, Brand = "Honda" }
        };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehiclesByUserIdAsync(userId))
            .ReturnsAsync(vehicles);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetVehicleDto>>(vehicles))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_WhenNoVehiclesForUser_ShouldReturnEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetAllVehicleByUserIdQuery { UserId = userId };

        var emptyList = new List<Domain.Entities.Vehicle>();

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehiclesByUserIdAsync(userId))
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetVehicleDto>>(emptyList))
            .Returns(new List<GetVehicleDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetAllVehicleByUserIdQuery { UserId = userId };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehiclesByUserIdAsync(userId))
            .ReturnsAsync(new List<Domain.Entities.Vehicle>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetVehicleDto>>(It.IsAny<IEnumerable<Domain.Entities.Vehicle>>()))
            .Returns(new List<GetVehicleDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _vehicleRepositoriesMock.Verify(x => x.GetVehiclesByUserIdAsync(userId), Times.Once);
    }
}
