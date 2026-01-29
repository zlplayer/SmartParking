using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Infrastructure.Repositories;
using Xunit;

namespace SmartParking.Infrastructure.Tests.Repositories;

[TestSubject(typeof(ParkingSpaceRepositories))]
public class ParkingSpaceRepositoriesTest : IDisposable
{
    private readonly SmartParkingDbContext _dbContext;
    private readonly ParkingSpaceRepositories _repository;

    public ParkingSpaceRepositoriesTest()
    {
        var options = new DbContextOptionsBuilder<SmartParkingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new SmartParkingDbContext(options);
        _repository = new ParkingSpaceRepositories(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    #region GetAllParkingSpaceAsync

    [Fact]
    public async Task GetAllParkingSpaceAsync_ShouldReturnAllParkingSpaces()
    {
        // Arrange
        var parkingSpaces = new List<ParkingSpace>
        {
            new() { Id = Guid.NewGuid(), Code = "A1", Status = "Available", Width = 2.5f, Height = 5.0f },
            new() { Id = Guid.NewGuid(), Code = "A2", Status = "Reserved", Width = 2.5f, Height = 5.0f },
            new() { Id = Guid.NewGuid(), Code = "A3", Status = "Available", Width = 2.5f, Height = 5.0f }
        };
        await _dbContext.ParkingSpaces.AddRangeAsync(parkingSpaces);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllParkingSpaceAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllParkingSpaceAsync_WhenNoParkingSpaces_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _repository.GetAllParkingSpaceAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetAllAvailableParkingSpaceAsync

    [Fact]
    public async Task GetAllAvailableParkingSpaceAsync_ShouldReturnOnlyAvailableParkingSpaces()
    {
        // Arrange
        var parkingSpaces = new List<ParkingSpace>
        {
            new() { Id = Guid.NewGuid(), Code = "A1", Status = "Available", Width = 2.5f, Height = 5.0f },
            new() { Id = Guid.NewGuid(), Code = "A2", Status = "Reserved", Width = 2.5f, Height = 5.0f },
            new() { Id = Guid.NewGuid(), Code = "A3", Status = "Available", Width = 2.5f, Height = 5.0f },
            new() { Id = Guid.NewGuid(), Code = "A4", Status = "Occupied", Width = 2.5f, Height = 5.0f }
        };
        await _dbContext.ParkingSpaces.AddRangeAsync(parkingSpaces);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAvailableParkingSpaceAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, ps => Assert.Equal("Available", ps.Status));
    }

    [Fact]
    public async Task GetAllAvailableParkingSpaceAsync_WhenNoAvailableParkingSpaces_ShouldReturnEmptyCollection()
    {
        // Arrange
        var parkingSpaces = new List<ParkingSpace>
        {
            new() { Id = Guid.NewGuid(), Code = "A1", Status = "Reserved", Width = 2.5f, Height = 5.0f },
            new() { Id = Guid.NewGuid(), Code = "A2", Status = "Occupied", Width = 2.5f, Height = 5.0f }
        };
        await _dbContext.ParkingSpaces.AddRangeAsync(parkingSpaces);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAvailableParkingSpaceAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetParkingSpaceByIdAsync

    [Fact]
    public async Task GetParkingSpaceByIdAsync_WhenParkingSpaceExists_ShouldReturnParkingSpace()
    {
        // Arrange
        var parkingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = "B1",
            Status = "Available",
            Width = 3.0f,
            Height = 6.0f
        };
        await _dbContext.ParkingSpaces.AddAsync(parkingSpace);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetParkingSpaceByIdAsync(parkingSpace.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(parkingSpace.Id, result.Id);
        Assert.Equal("B1", result.Code);
        Assert.Equal("Available", result.Status);
    }

    [Fact]
    public async Task GetParkingSpaceByIdAsync_WhenParkingSpaceNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetParkingSpaceByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region CreateParkingSpaceAsync

    [Fact]
    public async Task CreateParkingSpaceAsync_ShouldAddParkingSpaceToDatabase()
    {
        // Arrange
        var parkingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = "C1",
            Status = "Available",
            Width = 2.5f,
            Height = 5.0f,
            ParkingZoneId = Guid.NewGuid()
        };

        // Act
        await _repository.CreateParkingSpaceAsync(parkingSpace);

        // Assert
        var savedSpace = await _dbContext.ParkingSpaces.FindAsync(parkingSpace.Id);
        Assert.NotNull(savedSpace);
        Assert.Equal("C1", savedSpace.Code);
        Assert.Equal("Available", savedSpace.Status);
    }

    #endregion

    #region UpdateParkingSpaceAsync

    [Fact]
    public async Task UpdateParkingSpaceAsync_ShouldUpdateParkingSpaceInDatabase()
    {
        // Arrange
        var parkingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = "D1",
            Status = "Available",
            Width = 2.5f,
            Height = 5.0f
        };
        await _dbContext.ParkingSpaces.AddAsync(parkingSpace);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(parkingSpace).State = EntityState.Detached;

        // Act
        parkingSpace.Code = "D1-Updated";
        parkingSpace.Status = "Reserved";
        await _repository.UpdateParkingSpaceAsync(parkingSpace);

        // Assert
        var updatedSpace = await _dbContext.ParkingSpaces.FindAsync(parkingSpace.Id);
        Assert.NotNull(updatedSpace);
        Assert.Equal("D1-Updated", updatedSpace.Code);
        Assert.Equal("Reserved", updatedSpace.Status);
    }

    #endregion

    #region DeleteParkingSpaceAsync

    [Fact]
    public async Task DeleteParkingSpaceAsync_ShouldRemoveParkingSpaceFromDatabase()
    {
        // Arrange
        var parkingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = "E1",
            Status = "Available",
            Width = 2.5f,
            Height = 5.0f
        };
        await _dbContext.ParkingSpaces.AddAsync(parkingSpace);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteParkingSpaceAsync(parkingSpace);

        // Assert
        var deletedSpace = await _dbContext.ParkingSpaces.FindAsync(parkingSpace.Id);
        Assert.Null(deletedSpace);
    }

    #endregion
}
