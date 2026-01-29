using System;
using JetBrains.Annotations;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Domain.Tests.Entities;

[TestSubject(typeof(User))]
public class UserTest
{
    [Fact]
    public void User_ShouldCreateInstance()
    {
        var user = new User();

        Assert.NotNull(user);
    }

    [Fact]
    public void User_ShouldSetAndGetProperties()
    {
        var id = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var user = new User
        {
            Id = id,
            Email = "test@example.com",
            FirstName = "Jan",
            LastName = "Kowalski",
            Username = "jkowalski",
            PasswordHash = "hashedpassword123",
            RoleId = roleId
        };

        Assert.Equal(id, user.Id);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("Jan", user.FirstName);
        Assert.Equal("Kowalski", user.LastName);
        Assert.Equal("jkowalski", user.Username);
        Assert.Equal("hashedpassword123", user.PasswordHash);
        Assert.Equal(roleId, user.RoleId);
    }

    [Fact]
    public void User_RoleId_ShouldHaveDefaultValue()
    {
        var user = new User();

        var expectedDefaultRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        Assert.Equal(expectedDefaultRoleId, user.RoleId);
    }

    [Fact]
    public void User_VehiclesList_ShouldBeInitializedAsEmptyList()
    {
        var user = new User();

        Assert.NotNull(user.Vehicles);
        Assert.Empty(user.Vehicles);
    }

    [Fact]
    public void User_ParkingZonesList_ShouldBeInitializedAsEmptyList()
    {
        var user = new User();

        Assert.NotNull(user.ParkingZones);
        Assert.Empty(user.ParkingZones);
    }

    [Fact]
    public void User_ReservationsList_ShouldBeInitializedAsEmptyList()
    {
        var user = new User();

        Assert.NotNull(user.Reservations);
        Assert.Empty(user.Reservations);
    }

    [Fact]
    public void User_ShouldAllowAddingVehicles()
    {
        var user = new User();
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Brand = "BMW" };

        user.Vehicles.Add(vehicle);

        Assert.Single(user.Vehicles);
        Assert.Contains(vehicle, user.Vehicles);
    }

    [Fact]
    public void User_ShouldAllowAddingParkingZones()
    {
        var user = new User();
        var parkingZone = new ParkingZone { Id = Guid.NewGuid(), Name = "Strefa A" };

        user.ParkingZones.Add(parkingZone);

        Assert.Single(user.ParkingZones);
        Assert.Contains(parkingZone, user.ParkingZones);
    }

    [Fact]
    public void User_ShouldAllowAddingReservations()
    {
        var user = new User();
        var reservation = new Reservation { Id = Guid.NewGuid() };

        user.Reservations.Add(reservation);

        Assert.Single(user.Reservations);
        Assert.Contains(reservation, user.Reservations);
    }

    [Fact]
    public void User_ShouldAllowSettingRole()
    {
        var role = new Role { Id = Guid.NewGuid(), Name = "Admin" };
        var user = new User
        {
            RoleId = role.Id,
            Role = role
        };

        Assert.Equal(role, user.Role);
        Assert.Equal(role.Id, user.RoleId);
    }
}
