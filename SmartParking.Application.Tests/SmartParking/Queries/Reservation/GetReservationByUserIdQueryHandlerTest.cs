using AutoMapper;
using JetBrains.Annotations;
using Moq;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Queries.GetReservationByUserId;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.SmartParking.Queries.Reservation;

[TestSubject(typeof(GetReservationByUserIdQueryHandler))]
public class GetReservationByUserIdQueryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IReservationRepositories> _reservationRepositoriesMock;
    private readonly GetReservationByUserIdQueryHandler _handler;

    public GetReservationByUserIdQueryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _reservationRepositoriesMock = new Mock<IReservationRepositories>();
        _handler = new GetReservationByUserIdQueryHandler(
            _mapperMock.Object,
            _reservationRepositoriesMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnReservationsForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetReservationByUserIdQuery { UserId = userId };

        var reservations = new List<Domain.Entities.Reservation>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Status = "Active" },
            new() { Id = Guid.NewGuid(), UserId = userId, Status = "Completed" }
        };

        var expectedDtos = new List<GetReservationDto>
        {
            new() { Id = reservations[0].Id, Status = "Active" },
            new() { Id = reservations[1].Id, Status = "Completed" }
        };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationsByUserIdAsync(userId))
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
    public async Task Handle_WhenNoReservationsForUser_ShouldReturnEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetReservationByUserIdQuery { UserId = userId };

        var emptyList = new List<Domain.Entities.Reservation>();

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationsByUserIdAsync(userId))
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
    public async Task Handle_ShouldCallRepositoryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetReservationByUserIdQuery { UserId = userId };

        _reservationRepositoriesMock
            .Setup(x => x.GetReservationsByUserIdAsync(userId))
            .ReturnsAsync(new List<Domain.Entities.Reservation>());

        _mapperMock
            .Setup(x => x.Map<IEnumerable<GetReservationDto>>(It.IsAny<IEnumerable<Domain.Entities.Reservation>>()))
            .Returns(new List<GetReservationDto>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _reservationRepositoriesMock.Verify(x => x.GetReservationsByUserIdAsync(userId), Times.Once);
    }
}
