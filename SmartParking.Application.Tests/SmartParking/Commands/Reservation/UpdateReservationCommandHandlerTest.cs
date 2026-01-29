using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.UpdateReservation;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Reservation;

[TestSubject(typeof(UpdateReservationCommandHandler))]
public class UpdateReservationCommandHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly UpdateReservationCommandHandler _handler;

    public UpdateReservationCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _handler = new UpdateReservationCommandHandler(
            _mapperMock.Object,
            _reservationRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenReservationExists_ShouldUpdateReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new UpdateReservationCommand
        {
            Id = reservationId,
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(3),
            Status = "Confirmed"
        };

        var existingReservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            Status = "Pending"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(existingReservation);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, existingReservation), Times.Once);
        _reservationRepositoriesMock.Verify(x => x.UpdateReservationAsync(existingReservation), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenReservationNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateReservationCommand
        {
            Id = Guid.NewGuid(),
            Status = "Confirmed"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Reservation?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new UpdateReservationCommand
        {
            Id = reservationId,
            Status = "Test"
        };

        var existingReservation = new Domain.Entities.Reservation { Id = reservationId };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(existingReservation);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.GetReservationByIdAsync(reservationId), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenReservationNotFound_ShouldNotCallUpdateMethod()
    {
        // Arrange
        var command = new UpdateReservationCommand
        {
            Id = Guid.NewGuid(),
            Status = "Confirmed"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Reservation?)null);

        // Act & Assert
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (KeyNotFoundException)
        {
            // Expected
        }

        _reservationRepositoriesMock.Verify(
            x => x.UpdateReservationAsync(It.IsAny<Domain.Entities.Reservation>()),
            Times.Never);
    }
}
