using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CancelReservation;
using SmartParking.Application.SmartParking.Commands.CreateReservation;
using SmartParking.Application.SmartParking.Commands.DeleteReservation;
using SmartParking.Application.SmartParking.Commands.UpdateReservation;
using SmartParking.Application.SmartParking.Queries.GetAllReservations;
using SmartParking.Application.SmartParking.Queries.GetReservationById;
using SmartParking.Application.SmartParking.Queries.GetReservationByUserId;
using SmartParking.Server.Controllers;
using Xunit;

namespace SmartParking.Server.Tests.Controllers;

[TestSubject(typeof(ReservationController))]
public class ReservationControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReservationController _controller;

    public ReservationControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ReservationController(_mediatorMock.Object);
    }

    #region GetAllReservation

    [Fact]
    public async Task GetAllReservation_ShouldReturnOk_WithReservations()
    {
        // Arrange
        var reservations = new List<GetReservationDto>
        {
            new() { Id = Guid.NewGuid(), Status = "Active" },
            new() { Id = Guid.NewGuid(), Status = "Completed" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservations);

        // Act
        var result = await _controller.GetAllReservation();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReservations = Assert.IsAssignableFrom<IEnumerable<GetReservationDto>>(okResult.Value);
        Assert.Equal(2, returnedReservations.Count());
    }

    [Fact]
    public async Task GetAllReservation_ShouldReturnEmptyList_WhenNoReservations()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetReservationDto>());

        // Act
        var result = await _controller.GetAllReservation();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReservations = Assert.IsAssignableFrom<IEnumerable<GetReservationDto>>(okResult.Value);
        Assert.Empty(returnedReservations);
    }

    [Fact]
    public async Task GetAllReservation_ShouldSendQueryToMediator()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetReservationDto>());

        // Act
        await _controller.GetAllReservation();

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllReservationQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetReservationById

    [Fact]
    public async Task GetReservationById_ShouldReturnOk_WithReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var reservation = new GetReservationDto
        {
            Id = reservationId,
            Status = "Active"
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetReservationByIdQuery>(q => q.Id == reservationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservation);

        // Act
        var result = await _controller.GetReservationById(reservationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReservation = Assert.IsType<GetReservationDto>(okResult.Value);
        Assert.Equal(reservationId, returnedReservation.Id);
    }

    [Fact]
    public async Task GetReservationById_ShouldSendQueryWithCorrectId()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetReservationByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetReservationDto());

        // Act
        await _controller.GetReservationById(reservationId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetReservationByIdQuery>(q => q.Id == reservationId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetReservationByUserId

    [Fact]
    public async Task GetReservationByUserId_ShouldReturnOk_WithReservations()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var reservations = new List<GetReservationDto>
        {
            new() { Id = Guid.NewGuid(), Status = "Active" },
            new() { Id = Guid.NewGuid(), Status = "Pending" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetReservationByUserIdQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservations);

        // Act
        var result = await _controller.GetReservationByUserId(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedReservations = Assert.IsAssignableFrom<IEnumerable<GetReservationDto>>(okResult.Value);
        Assert.Equal(2, returnedReservations.Count());
    }

    [Fact]
    public async Task GetReservationByUserId_ShouldSendQueryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetReservationByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetReservationDto>());

        // Act
        await _controller.GetReservationByUserId(userId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetReservationByUserIdQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region CreateReservation

    [Fact]
    public async Task CreateReservation_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateReservationCommand
        {
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = Guid.NewGuid(),
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2)
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CreateReservation(command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateReservation_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new CreateReservationCommand
        {
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = Guid.NewGuid()
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.CreateReservation(command);

        // Assert
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateReservation

    [Fact]
    public async Task UpdateReservation_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new UpdateReservationCommand
        {
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(3)
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateReservationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CreateReservation(reservationId, command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateReservation_ShouldSetIdFromRoute()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new UpdateReservationCommand();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateReservationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.CreateReservation(reservationId, command);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateReservationCommand>(c => c.Id == reservationId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region CancelReservation

    [Fact]
    public async Task CancelReservation_ShouldReturnOk_WhenCommandIsValid()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new CancelReservationCommand
        {
            CancellationReason = "User requested"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CancelReservationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CancelReservation(reservationId, command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CancelReservation_ShouldSetIdFromRoute()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new CancelReservationCommand
        {
            CancellationReason = "Changed plans"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CancelReservationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.CancelReservation(reservationId, command);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<CancelReservationCommand>(c => c.Id == reservationId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region DeleteReservation

    [Fact]
    public async Task DeleteReservation_ShouldReturnOk_WhenReservationExists()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteReservationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.DeleteReservation(reservationId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteReservation_ShouldSendCommandWithCorrectId()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteReservationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        await _controller.DeleteReservation(reservationId);

        // Assert
        _mediatorMock.Verify(
            m => m.Send(It.Is<DeleteReservationCommand>(c => c.Id == reservationId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
