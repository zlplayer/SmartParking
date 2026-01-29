using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Infrastructure.Repositories;
using Xunit;

namespace SmartParking.Infrastructure.Tests.Repositories;

[TestSubject(typeof(ReservationRepositories))]
public class ReservationRepositoriesTest : IDisposable
{
    private readonly SmartParkingDbContext _dbContext;
    private readonly ReservationRepositories _repository;

    public ReservationRepositoriesTest()
    {
        var options = new DbContextOptionsBuilder<SmartParkingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new SmartParkingDbContext(options);
        _repository = new ReservationRepositories(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    #region GetAllReservationsAsync

    [Fact]
    public async Task GetAllReservationsAsync_ShouldReturnAllReservations()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new() { Id = Guid.NewGuid(), Status = "Active", BookedFrom = DateTime.UtcNow, BookedUntil = DateTime.UtcNow.AddHours(2) },
            new() { Id = Guid.NewGuid(), Status = "Completed", BookedFrom = DateTime.UtcNow, BookedUntil = DateTime.UtcNow.AddHours(2) },
            new() { Id = Guid.NewGuid(), Status = "Cancelled", BookedFrom = DateTime.UtcNow, BookedUntil = DateTime.UtcNow.AddHours(2) }
        };
        await _dbContext.Reservations.AddRangeAsync(reservations);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllReservationsAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllReservationsAsync_WhenNoReservations_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _repository.GetAllReservationsAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetReservationsByUserIdAsync

    [Fact]
    public async Task GetReservationsByUserIdAsync_ShouldReturnReservationsForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var reservations = new List<Reservation>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Status = "Active", BookedFrom = DateTime.UtcNow, BookedUntil = DateTime.UtcNow.AddHours(2) },
            new() { Id = Guid.NewGuid(), UserId = userId, Status = "Completed", BookedFrom = DateTime.UtcNow, BookedUntil = DateTime.UtcNow.AddHours(2) },
            new() { Id = Guid.NewGuid(), UserId = otherUserId, Status = "Active", BookedFrom = DateTime.UtcNow, BookedUntil = DateTime.UtcNow.AddHours(2) }
        };
        await _dbContext.Reservations.AddRangeAsync(reservations);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetReservationsByUserIdAsync(userId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, r => Assert.Equal(userId, r.UserId));
    }

    [Fact]
    public async Task GetReservationsByUserIdAsync_WhenNoReservationsForUser_ShouldReturnEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            UserId = otherUserId,
            Status = "Active",
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2)
        };
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetReservationsByUserIdAsync(userId);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetReservationByIdAsync

    [Fact]
    public async Task GetReservationByIdAsync_WhenReservationExists_ShouldReturnReservation()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Active",
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            CreationDate = DateTime.UtcNow
        };
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetReservationByIdAsync(reservation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservation.Id, result.Id);
        Assert.Equal("Active", result.Status);
    }

    [Fact]
    public async Task GetReservationByIdAsync_WhenReservationNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetReservationByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region CreateReservationAsync

    [Fact]
    public async Task CreateReservationAsync_ShouldAddReservationToDatabase()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Pending",
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(3),
            CreationDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = Guid.NewGuid()
        };

        // Act
        await _repository.CreateReservationAsync(reservation);

        // Assert
        var savedReservation = await _dbContext.Reservations.FindAsync(reservation.Id);
        Assert.NotNull(savedReservation);
        Assert.Equal("Pending", savedReservation.Status);
    }

    #endregion

    #region UpdateReservationAsync

    [Fact]
    public async Task UpdateReservationAsync_ShouldUpdateReservationInDatabase()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Pending",
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2)
        };
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(reservation).State = EntityState.Detached;

        // Act
        reservation.Status = "Confirmed";
        reservation.ConfirmationDate = DateTime.UtcNow;
        await _repository.UpdateReservationAsync(reservation);

        // Assert
        var updatedReservation = await _dbContext.Reservations.FindAsync(reservation.Id);
        Assert.NotNull(updatedReservation);
        Assert.Equal("Confirmed", updatedReservation.Status);
    }

    [Fact]
    public async Task UpdateReservationAsync_ShouldUpdateCancellationFields()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Active",
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2)
        };
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(reservation).State = EntityState.Detached;

        // Act
        reservation.Status = "Cancelled";
        reservation.CancellationDate = DateTime.UtcNow;
        reservation.CancellationReason = "User requested";
        await _repository.UpdateReservationAsync(reservation);

        // Assert
        var updatedReservation = await _dbContext.Reservations.FindAsync(reservation.Id);
        Assert.NotNull(updatedReservation);
        Assert.Equal("Cancelled", updatedReservation.Status);
        Assert.NotNull(updatedReservation.CancellationDate);
        Assert.Equal("User requested", updatedReservation.CancellationReason);
    }

    #endregion

    #region DeleteReservationAsync

    [Fact]
    public async Task DeleteReservationAsync_ShouldRemoveReservationFromDatabase()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Active",
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2)
        };
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteReservationAsync(reservation);

        // Assert
        var deletedReservation = await _dbContext.Reservations.FindAsync(reservation.Id);
        Assert.Null(deletedReservation);
    }

    #endregion
}
