using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.CreateVehicle;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Vehicle;

[TestSubject(typeof(CreateVehicleCommandHandler))]
public class CreateVehicleCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IVehicleRepositories> _vehicleRepositoriesMock;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _vehicleRepositoriesMock = new Mock<IVehicleRepositories>();
        _handler = new CreateVehicleCommandHandler(
            _mapperMock.Object,
            _vehicleRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateVehicle()
    {
        // Arrange
        var command = new CreateVehicleCommand
        {
            Brand = "Toyota",
            Model = "Corolla",
            RegistrationNumber = "WA12345",
            CountryCode = "PL",
            UserId = Guid.NewGuid()
        };

        var vehicle = new Domain.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = command.Brand,
            Model = command.Model,
            RegistrationNumber = command.RegistrationNumber
        };

        _mapperMock
            .Setup(x => x.Map<CreateVehicleCommand, Domain.Entities.Vehicle>(command))
            .Returns(vehicle);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _vehicleRepositoriesMock.Verify(x => x.CreateVehicleAsync(vehicle), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToEntity()
    {
        // Arrange
        var command = new CreateVehicleCommand
        {
            Brand = "Honda",
            Model = "Civic",
            RegistrationNumber = "KR54321",
            CountryCode = "PL",
            UserId = Guid.NewGuid()
        };

        _mapperMock
            .Setup(x => x.Map<CreateVehicleCommand, Domain.Entities.Vehicle>(command))
            .Returns(new Domain.Entities.Vehicle());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<CreateVehicleCommand, Domain.Entities.Vehicle>(command), Times.Once);
    }
}
