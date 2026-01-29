using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Infrastructure.Repositories;
using Xunit;

namespace SmartParking.Infrastructure.Tests.Repositories;

[TestSubject(typeof(VehicleRepositories))]
public class VehicleRepositoriesTest : IDisposable
{
    private readonly SmartParkingDbContext _dbContext;
    private readonly VehicleRepositories _repository;

    public VehicleRepositoriesTest()
    {
        var options = new DbContextOptionsBuilder<SmartParkingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new SmartParkingDbContext(options);
        _repository = new VehicleRepositories(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    #region GetAllVehiclesAsync

    [Fact]
    public async Task GetAllVehiclesAsync_ShouldReturnAllVehicles()
    {
        // Arrange
        var vehicles = new List<Vehicle>
        {
            new() { Id = Guid.NewGuid(), Brand = "Toyota", Model = "Corolla", RegistrationNumber = "WA1111", CountryCode = "PL" },
            new() { Id = Guid.NewGuid(), Brand = "Honda", Model = "Civic", RegistrationNumber = "WA2222", CountryCode = "PL" }
        };
        await _dbContext.Vehicles.AddRangeAsync(vehicles);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllVehiclesAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllVehiclesAsync_WhenNoVehicles_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _repository.GetAllVehiclesAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetVehicleAsync

    [Fact]
    public async Task GetVehicleAsync_WhenVehicleExists_ShouldReturnVehicle()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "Toyota",
            Model = "Camry",
            RegistrationNumber = "WA3333",
            CountryCode = "PL"
        };
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetVehicleAsync(vehicle.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicle.Id, result.Id);
        Assert.Equal("Toyota", result.Brand);
        Assert.Equal("Camry", result.Model);
    }

    [Fact]
    public async Task GetVehicleAsync_WhenVehicleNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetVehicleAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetVehiclesByUserIdAsync

    [Fact]
    public async Task GetVehiclesByUserIdAsync_ShouldReturnVehiclesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var vehicles = new List<Vehicle>
        {
            new() { Id = Guid.NewGuid(), Brand = "Toyota", Model = "Corolla", UserId = userId, RegistrationNumber = "WA1111", CountryCode = "PL" },
            new() { Id = Guid.NewGuid(), Brand = "Honda", Model = "Civic", UserId = userId, RegistrationNumber = "WA2222", CountryCode = "PL" },
            new() { Id = Guid.NewGuid(), Brand = "BMW", Model = "3 Series", UserId = otherUserId, RegistrationNumber = "WA3333", CountryCode = "PL" }
        };
        await _dbContext.Vehicles.AddRangeAsync(vehicles);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetVehiclesByUserIdAsync(userId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, v => Assert.Equal(userId, v.UserId));
    }

    [Fact]
    public async Task GetVehiclesByUserIdAsync_WhenNoVehiclesForUser_ShouldReturnEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "BMW",
            Model = "5 Series",
            UserId = otherUserId,
            RegistrationNumber = "WA4444",
            CountryCode = "PL"
        };
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetVehiclesByUserIdAsync(userId);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region CreateVehicleAsync

    [Fact]
    public async Task CreateVehicleAsync_ShouldAddVehicleToDatabase()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "Mazda",
            Model = "6",
            RegistrationNumber = "WA5555",
            CountryCode = "PL",
            UserId = Guid.NewGuid()
        };

        // Act
        await _repository.CreateVehicleAsync(vehicle);

        // Assert
        var savedVehicle = await _dbContext.Vehicles.FindAsync(vehicle.Id);
        Assert.NotNull(savedVehicle);
        Assert.Equal("Mazda", savedVehicle.Brand);
        Assert.Equal("6", savedVehicle.Model);
    }

    #endregion

    #region UpdateVehicleAsync

    [Fact]
    public async Task UpdateVehicleAsync_ShouldUpdateVehicleInDatabase()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "Original Brand",
            Model = "Original Model",
            RegistrationNumber = "WA6666",
            CountryCode = "PL"
        };
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(vehicle).State = EntityState.Detached;

        // Act
        vehicle.Brand = "Updated Brand";
        vehicle.Model = "Updated Model";
        await _repository.UpdateVehicleAsync(vehicle);

        // Assert
        var updatedVehicle = await _dbContext.Vehicles.FindAsync(vehicle.Id);
        Assert.NotNull(updatedVehicle);
        Assert.Equal("Updated Brand", updatedVehicle.Brand);
        Assert.Equal("Updated Model", updatedVehicle.Model);
    }

    #endregion

    #region DeleteVehicleAsync

    [Fact]
    public async Task DeleteVehicleAsync_ShouldRemoveVehicleFromDatabase()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "To Delete",
            Model = "Delete Model",
            RegistrationNumber = "WA7777",
            CountryCode = "PL"
        };
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteVehicleAsync(vehicle);

        // Assert
        var deletedVehicle = await _dbContext.Vehicles.FindAsync(vehicle.Id);
        Assert.Null(deletedVehicle);
    }

    #endregion
}
