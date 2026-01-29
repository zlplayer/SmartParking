using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.CreateParkingSpace;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.ParkingSpace;

[TestSubject(typeof(CreateParkingSpaceCommandHandler))]
public class CreateParkingSpaceCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly CreateParkingSpaceCommandHandler _handler;

    public CreateParkingSpaceCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new CreateParkingSpaceCommandHandler(
            _mapperMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateParkingSpace()
    {
        // Arrange
        var command = new CreateParkingSpaceCommand
        {
            Code = "A1",
            Width = 2.5f,
            Height = 5.0f,
            Status = "Available",
            ParkingZoneId = Guid.NewGuid()
        };

        var parkingSpace = new Domain.Entities.ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = command.Code,
            Width = command.Width,
            Height = command.Height,
            Status = command.Status
        };

        _mapperMock
            .Setup(x => x.Map<CreateParkingSpaceCommand, Domain.Entities.ParkingSpace>(command))
            .Returns(parkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.CreateParkingSpaceAsync(parkingSpace), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToEntity()
    {
        // Arrange
        var command = new CreateParkingSpaceCommand
        {
            Code = "B2",
            Width = 3.0f,
            Height = 6.0f,
            Status = "Available",
            ParkingZoneId = Guid.NewGuid()
        };

        _mapperMock
            .Setup(x => x.Map<CreateParkingSpaceCommand, Domain.Entities.ParkingSpace>(command))
            .Returns(new Domain.Entities.ParkingSpace());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<CreateParkingSpaceCommand, Domain.Entities.ParkingSpace>(command), Times.Once);
    }
}
