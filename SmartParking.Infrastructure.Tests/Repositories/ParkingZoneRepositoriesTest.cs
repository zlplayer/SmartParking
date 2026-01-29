using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Infrastructure.Repositories;
using Xunit;

namespace SmartParking.Infrastructure.Tests.Repositories;

[TestSubject(typeof(ParkingZoneRepositories))]
public class ParkingZoneRepositoriesTest : IDisposable
{
    private readonly SmartParkingDbContext _dbContext;
    private readonly ParkingZoneRepositories _repository;

    public ParkingZoneRepositoriesTest()
    {
        var options = new DbContextOptionsBuilder<SmartParkingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new SmartParkingDbContext(options);
        _repository = new ParkingZoneRepositories(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    #region GetAllParkingzonesAsync

    [Fact]
    public async Task GetAllParkingzonesAsync_ShouldReturnAllParkingZones()
    {
        // Arrange
        var parkingZones = new List<ParkingZone>
        {
            new() { Id = Guid.NewGuid(), Name = "Zone A", City = "Warsaw", TariffType = "Standard", Regulamin = "Rules A" },
            new() { Id = Guid.NewGuid(), Name = "Zone B", City = "Krakow", TariffType = "Standard", Regulamin = "Rules B" },
            new() { Id = Guid.NewGuid(), Name = "Zone C", City = "Gdansk", TariffType = "Standard", Regulamin = "Rules C" }
        };
        await _dbContext.ParkingZones.AddRangeAsync(parkingZones);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllParkingzonesAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllParkingzonesAsync_WhenNoParkingZones_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _repository.GetAllParkingzonesAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetParkingZoneAsync

    [Fact]
    public async Task GetParkingZoneAsync_WhenParkingZoneExists_ShouldReturnParkingZone()
    {
        // Arrange
        var parkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "Test Zone",
            City = "Test City",
            TariffType = "Standard",
            Regulamin = "Test rules",
            IsActive = true
        };
        await _dbContext.ParkingZones.AddAsync(parkingZone);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetParkingZoneAsync(parkingZone.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(parkingZone.Id, result.Id);
        Assert.Equal("Test Zone", result.Name);
        Assert.Equal("Test City", result.City);
    }

    [Fact]
    public async Task GetParkingZoneAsync_WhenParkingZoneNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetParkingZoneAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region CreateParkingZoneAsync

    [Fact]
    public async Task CreateParkingZoneAsync_ShouldAddParkingZoneToDatabase()
    {
        // Arrange
        var parkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "New Zone",
            City = "New City",
            TariffType = "Premium",
            Regulamin = "Premium rules",
            IsActive = true
        };

        // Act
        await _repository.CreateParkingZoneAsync(parkingZone);

        // Assert
        var savedZone = await _dbContext.ParkingZones.FindAsync(parkingZone.Id);
        Assert.NotNull(savedZone);
        Assert.Equal("New Zone", savedZone.Name);
        Assert.Equal("New City", savedZone.City);
    }

    #endregion

    #region UpdateParkingZoneAsync

    [Fact]
    public async Task UpdateParkingZoneAsync_ShouldUpdateParkingZoneInDatabase()
    {
        // Arrange
        var parkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "Original Zone",
            City = "Original City",
            TariffType = "Standard",
            Regulamin = "Original rules",
            IsActive = true
        };
        await _dbContext.ParkingZones.AddAsync(parkingZone);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(parkingZone).State = EntityState.Detached;

        // Act
        parkingZone.Name = "Updated Zone";
        parkingZone.City = "Updated City";
        parkingZone.IsActive = false;
        await _repository.UpdateParkingZoneAsync(parkingZone);

        // Assert
        var updatedZone = await _dbContext.ParkingZones.FindAsync(parkingZone.Id);
        Assert.NotNull(updatedZone);
        Assert.Equal("Updated Zone", updatedZone.Name);
        Assert.Equal("Updated City", updatedZone.City);
        Assert.False(updatedZone.IsActive);
    }

    #endregion

    #region DeleteParkingZoneAsync

    [Fact]
    public async Task DeleteParkingZoneAsync_ShouldRemoveParkingZoneFromDatabase()
    {
        // Arrange
        var parkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            City = "Delete City",
            TariffType = "Standard",
            Regulamin = "Delete rules"
        };
        await _dbContext.ParkingZones.AddAsync(parkingZone);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteParkingZoneAsync(parkingZone);

        // Assert
        var deletedZone = await _dbContext.ParkingZones.FindAsync(parkingZone.Id);
        Assert.Null(deletedZone);
    }

    #endregion
}
