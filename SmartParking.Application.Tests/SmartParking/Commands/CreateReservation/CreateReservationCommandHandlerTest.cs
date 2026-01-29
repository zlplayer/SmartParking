using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.CreateReservation;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.CreateReservation;

[TestSubject(typeof(CreateReservationCommandHandler))]
public class CreateReservationCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly CreateReservationCommandHandler _handler;

    public CreateReservationCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new CreateReservationCommandHandler(
            _mapperMock.Object,
            _reservationRepositoriesMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceExists_ShouldCreateReservationAndUpdateParkingSpaceStatus()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new CreateReservationCommand
        {
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            Status = "Pending",
            CreationDate = DateTime.UtcNow,
            ConfirmationDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = parkingSpaceId
        };

        var reservation = new Domain.Entities.Reservation
        {
            Id = Guid.NewGuid(),
            ParkingSpaceId = parkingSpaceId
        };

        var parkingSpace = new Domain.Entities.ParkingSpace
        {
            Id = parkingSpaceId,
            Status = "Available",
            Code = "A1"
        };

        _mapperMock
            .Setup(x => x.Map<CreateReservationCommand, Domain.Entities.Reservation>(command))
            .Returns(reservation);

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync(parkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.CreateReservationAsync(reservation), Times.Once);
        _parkingSpaceRepositoriesMock.Verify(x => x.UpdateParkingSpaceAsync(parkingSpace), Times.Once);
        Assert.Equal("Reserved", parkingSpace.Status);
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceNotFound_ShouldCreateReservationButNotUpdateParkingSpace()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new CreateReservationCommand
        {
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            Status = "Pending",
            CreationDate = DateTime.UtcNow,
            ConfirmationDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = parkingSpaceId
        };

        var reservation = new Domain.Entities.Reservation
        {
            Id = Guid.NewGuid(),
            ParkingSpaceId = parkingSpaceId
        };

        _mapperMock
            .Setup(x => x.Map<CreateReservationCommand, Domain.Entities.Reservation>(command))
            .Returns(reservation);

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(parkingSpaceId))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.CreateReservationAsync(reservation), Times.Once);
        _parkingSpaceRepositoriesMock.Verify(x => x.UpdateParkingSpaceAsync(It.IsAny<Domain.Entities.ParkingSpace>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToReservationEntity()
    {
        // Arrange
        var command = new CreateReservationCommand
        {
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            Status = "Pending",
            CreationDate = DateTime.UtcNow,
            ConfirmationDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = Guid.NewGuid()
        };

        var reservation = new Domain.Entities.Reservation();

        _mapperMock
            .Setup(x => x.Map<CreateReservationCommand, Domain.Entities.Reservation>(command))
            .Returns(reservation);

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<CreateReservationCommand, Domain.Entities.Reservation>(command), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectParkingSpaceId()
    {
        // Arrange
        var parkingSpaceId = Guid.NewGuid();
        var command = new CreateReservationCommand
        {
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            Status = "Pending",
            CreationDate = DateTime.UtcNow,
            ConfirmationDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = parkingSpaceId
        };

        _mapperMock
            .Setup(x => x.Map<CreateReservationCommand, Domain.Entities.Reservation>(command))
            .Returns(new Domain.Entities.Reservation());

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _parkingSpaceRepositoriesMock.Verify(x => x.GetParkingSpaceByIdAsync(parkingSpaceId), Times.Once);
    }
}
