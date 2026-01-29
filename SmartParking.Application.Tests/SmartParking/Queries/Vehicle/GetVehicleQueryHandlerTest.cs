using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetVehicle;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.Vehicle;

[TestSubject(typeof(GetVehicleQueryHandler))]
public class GetVehicleQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IVehicleRepositories> _vehicleRepositoriesMock;
    private readonly GetVehicleQueryHandler _handler;

    public GetVehicleQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _vehicleRepositoriesMock = new Mock<IVehicleRepositories>();
        _handler = new GetVehicleQueryHandler(
            _mapperMock.Object,
            _vehicleRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenVehicleExists_ShouldReturnDto()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var query = new GetVehicleQuery { VehicleId = vehicleId };

        var vehicle = new Domain.Entities.Vehicle
        {
            Id = vehicleId,
            Brand = "Toyota",
            Model = "Corolla"
        };

        var expectedDto = new GetVehicleDto
        {
            Id = vehicleId,
            Brand = "Toyota",
            Model = "Corolla"
        };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(x => x.Map<GetVehicleDto>(vehicle))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Brand, result.Brand);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var query = new GetVehicleQuery { VehicleId = vehicleId };

        var vehicle = new Domain.Entities.Vehicle { Id = vehicleId };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(x => x.Map<GetVehicleDto>(vehicle))
            .Returns(new GetVehicleDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _vehicleRepositoriesMock.Verify(x => x.GetVehicleAsync(vehicleId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapEntityToDto()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var query = new GetVehicleQuery { VehicleId = vehicleId };

        var vehicle = new Domain.Entities.Vehicle { Id = vehicleId };

        _vehicleRepositoriesMock
            .Setup(x => x.GetVehicleAsync(vehicleId))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(x => x.Map<GetVehicleDto>(vehicle))
            .Returns(new GetVehicleDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<GetVehicleDto>(vehicle), Times.Once);
    }
}
