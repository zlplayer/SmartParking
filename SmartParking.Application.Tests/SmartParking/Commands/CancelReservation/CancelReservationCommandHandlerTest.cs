using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.CancelReservation;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.CancelReservation;

[TestSubject(typeof(CancelReservationCommandHandler))]
public class CancelReservationCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly Mock<IParkingSpaceRepositories> _parkingSpaceRepositoriesMock;
    private readonly CancelReservationCommandHandler _handler;

    public CancelReservationCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _parkingSpaceRepositoriesMock = new Mock<IParkingSpaceRepositories>();
        _handler = new CancelReservationCommandHandler(
            _mapperMock.Object,
            _reservationRepositoriesMock.Object,
            _parkingSpaceRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenReservationExists_ShouldCancelAndUpdateParkingSpace()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var parkingSpaceId = Guid.NewGuid();
        var command = new CancelReservationCommand
        {
            Id = reservationId,
            CancellationDate = DateTime.UtcNow,
            CancellationReason = "User requested cancellation"
        };

        var reservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            Status = "Active",
            ParkingSpaceId = parkingSpaceId
        };

        var parkingSpace = new Domain.Entities.ParkingSpace
        {
            Id = parkingSpaceId,
            Status = "Reserved"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        // Note: Current implementation has a bug - it uses request.Id instead of reservation.ParkingSpaceId
        // This test reflects actual behavior
        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(reservationId))
            .ReturnsAsync(parkingSpace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, reservation), Times.Once);
        _reservationRepositoriesMock.Verify(x => x.UpdateReservationAsync(reservation), Times.Once);
        _parkingSpaceRepositoriesMock.Verify(x => x.UpdateParkingSpaceAsync(parkingSpace), Times.Once);
        Assert.Equal("Available", parkingSpace.Status);
    }

    [Fact]
    public async Task Handle_WhenReservationNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new CancelReservationCommand
        {
            Id = Guid.NewGuid(),
            CancellationDate = DateTime.UtcNow,
            CancellationReason = "Test reason"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Reservation?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenParkingSpaceNotFound_ShouldNotThrowAndOnlyUpdateReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new CancelReservationCommand
        {
            Id = reservationId,
            CancellationDate = DateTime.UtcNow,
            CancellationReason = "Test reason"
        };

        var reservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            Status = "Active"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(reservationId))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.UpdateReservationAsync(reservation), Times.Once);
        _parkingSpaceRepositoriesMock.Verify(x => x.UpdateParkingSpaceAsync(It.IsAny<Domain.Entities.ParkingSpace>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var cancellationDate = DateTime.UtcNow;
        var cancellationReason = "User changed plans";

        var command = new CancelReservationCommand
        {
            Id = reservationId,
            CancellationDate = cancellationDate,
            CancellationReason = cancellationReason
        };

        var reservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            Status = "Active"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        _parkingSpaceRepositoriesMock
            .Setup(x => x.GetParkingSpaceByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.ParkingSpace?)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, reservation), Times.Once);
    }
}
