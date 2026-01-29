using JetBrains.Annotations;
using Moq;
using SmartParking.Application.SmartParking.Commands.DeleteReservation;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Commands.Reservation;

[TestSubject(typeof(DeleteReservationCommandHandler))]
public class DeleteReservationCommandHandlerTest
{
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly DeleteReservationCommandHandler _handler;

    public DeleteReservationCommandHandlerTest()
    {
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _handler = new DeleteReservationCommandHandler(_reservationRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenReservationExists_ShouldDeleteReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new DeleteReservationCommand { Id = reservationId };

        var existingReservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            Status = "Active"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(existingReservation);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.DeleteReservationAsync(existingReservation), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenReservationNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteReservationCommand { Id = Guid.NewGuid() };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.Reservation?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldCallGetReservationWithCorrectId()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new DeleteReservationCommand { Id = reservationId };

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
    public async Task Handle_WhenReservationNotFound_ShouldNotCallDeleteMethod()
    {
        // Arrange
        var command = new DeleteReservationCommand { Id = Guid.NewGuid() };

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
            x => x.DeleteReservationAsync(It.IsAny<Domain.Entities.Reservation>()),
            Times.Never);
    }
}
