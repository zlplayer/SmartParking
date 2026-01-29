using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetAllReservations;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.Reservation;

[TestSubject(typeof(GetAllReservationQueryHandler))]
public class GetAllReservationQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly GetAllReservationQueryHandler _handler;

    public GetAllReservationQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _handler = new GetAllReservationQueryHandler(
            _mapperMock.Object,
            _reservationRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllReservations()
    {
        // Arrange
        var query = new GetAllReservationQuery();

        var reservations = new List<Domain.Entities.Reservation>
        {
            new() { Id = Guid.NewGuid(), Status = "Active" },
            new() { Id = Guid.NewGuid(), Status = "Completed" }
        };

        var expectedDtos = new List<GetReservationDto>
        {
            new() { Id = reservations[0].Id, Status = "Active" },
            new() { Id = reservations[1].Id, Status = "Completed" }
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetAllReservationsAsync())
            .ReturnsAsync(reservations);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetReservationDto>>(reservations))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_WhenNoReservations_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllReservationQuery();

        var emptyList = new List<Domain.Entities.Reservation>();

        _reservationRepositoriesMock
            .Setup(x => x.GetAllReservationsAsync())
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetReservationDto>>(emptyList))
            .Returns(new List<GetReservationDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldCallRepository()
    {
        // Arrange
        var query = new GetAllReservationQuery();

        _reservationRepositoriesMock
            .Setup(x => x.GetAllReservationsAsync())
            .ReturnsAsync(new List<Domain.Entities.Reservation>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetReservationDto>>(It.IsAny<IEnumerable<Domain.Entities.Reservation>>()))
            .Returns(new List<GetReservationDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.GetAllReservationsAsync(), Times.Once);
    }
}
