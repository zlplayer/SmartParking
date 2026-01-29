using System;
using JetBrains.Annotations;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Domain.Tests.Entities;

[TestSubject(typeof(Vehicle))]
public class VehicleTest
{
    [Fact]
    public void Vehicle_ShouldCreateInstance()
    {
        var vehicle = new Vehicle();

        Assert.NotNull(vehicle);
    }

    [Fact]
    public void Vehicle_ShouldSetAndGetProperties()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var vehicle = new Vehicle
        {
            Id = id,
            Brand = "BMW",
            Model = "M3",
            RegistrationNumber = "WA12345",
            CountryCode = "PL",
            UserId = userId
        };

        Assert.Equal(id, vehicle.Id);
        Assert.Equal("BMW", vehicle.Brand);
        Assert.Equal("M3", vehicle.Model);
        Assert.Equal("WA12345", vehicle.RegistrationNumber);
        Assert.Equal("PL", vehicle.CountryCode);
        Assert.Equal(userId, vehicle.UserId);
    }

    [Fact]
    public void Vehicle_ReservationsList_ShouldBeInitializedAsEmptyList()
    {
        var vehicle = new Vehicle();

        Assert.NotNull(vehicle.Reservations);
        Assert.Empty(vehicle.Reservations);
    }

    [Fact]
    public void Vehicle_ShouldAllowAddingReservations()
    {
        var vehicle = new Vehicle();
        var reservation = new Reservation { Id = Guid.NewGuid() };

        vehicle.Reservations.Add(reservation);

        Assert.Single(vehicle.Reservations);
        Assert.Contains(reservation, vehicle.Reservations);
    }

    [Fact]
    public void Vehicle_ShouldAllowSettingUser()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "testuser" };
        var vehicle = new Vehicle
        {
            UserId = user.Id,
            User = user
        };

        Assert.Equal(user, vehicle.User);
        Assert.Equal(user.Id, vehicle.UserId);
    }
}