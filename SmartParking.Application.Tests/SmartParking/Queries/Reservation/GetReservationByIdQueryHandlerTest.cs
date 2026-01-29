using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetReservationById;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.Reservation;

[TestSubject(typeof(GetReservationByIdQueryHandler))]
public class GetReservationByIdQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly GetReservationByIdQueryHandler _handler;

    public GetReservationByIdQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _handler = new GetReservationByIdQueryHandler(
            _mapperMock.Object,
            _reservationRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_WhenReservationExists_ShouldReturnDto()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var query = new GetReservationByIdQuery { Id = reservationId };

        var reservation = new Domain.Entities.Reservation
        {
            Id = reservationId,
            Status = "Active"
        };

        var expectedDto = new GetReservationDto
        {
            Id = reservationId,
            Status = "Active"
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        _mapperMock
            .Setup(x => x.Map<GetReservationDto>(reservation))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var query = new GetReservationByIdQuery { Id = reservationId };

        var reservation = new Domain.Entities.Reservation { Id = reservationId };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        _mapperMock
            .Setup(x => x.Map<GetReservationDto>(reservation))
            .Returns(new GetReservationDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.GetReservationByIdAsync(reservationId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapEntityToDto()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var query = new GetReservationByIdQuery { Id = reservationId };

        var reservation = new Domain.Entities.Reservation { Id = reservationId };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        _mapperMock
            .Setup(x => x.Map<GetReservationDto>(reservation))
            .Returns(new GetReservationDto());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map<GetReservationDto>(reservation), Times.Once);
    }
}
