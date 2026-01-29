using System;
using JetBrains.Annotations;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Domain.Tests.Entities;

[TestSubject(typeof(Reservation))]
public class ReservationTest
{
    [Fact]
    public void Reservation_ShouldCreateInstance()
    {
        var reservation = new Reservation();

        Assert.NotNull(reservation);
    }

    [Fact]
    public void Reservation_ShouldSetAndGetProperties()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var parkingSpaceId = Guid.NewGuid();
        var bookedFrom = new DateTime(2026, 2, 1, 8, 0, 0);
        var bookedUntil = new DateTime(2026, 2, 1, 18, 0, 0);
        var creationDate = new DateTime(2026, 1, 28, 10, 0, 0);
        var confirmationDate = new DateTime(2026, 1, 28, 10, 5, 0);

        var reservation = new Reservation
        {
            Id = id,
            BookedFrom = bookedFrom,
            BookedUntil = bookedUntil,
            Status = "Confirmed",
            CreationDate = creationDate,
            ConfirmationDate = confirmationDate,
            UserId = userId,
            VehicleId = vehicleId,
            ParkingSpaceId = parkingSpaceId
        };

        Assert.Equal(id, reservation.Id);
        Assert.Equal(bookedFrom, reservation.BookedFrom);
        Assert.Equal(bookedUntil, reservation.BookedUntil);
        Assert.Equal("Confirmed", reservation.Status);
        Assert.Equal(creationDate, reservation.CreationDate);
        Assert.Equal(confirmationDate, reservation.ConfirmationDate);
        Assert.Equal(userId, reservation.UserId);
        Assert.Equal(vehicleId, reservation.VehicleId);
        Assert.Equal(parkingSpaceId, reservation.ParkingSpaceId);
    }

    [Fact]
    public void Reservation_CancellationDate_ShouldBeNullable()
    {
        var reservation = new Reservation();

        Assert.Null(reservation.CancellationDate);
    }

    [Fact]
    public void Reservation_CancellationReason_ShouldBeNullable()
    {
        var reservation = new Reservation();

        Assert.Null(reservation.CancellationReason);
    }

    [Fact]
    public void Reservation_ShouldSetCancellationProperties()
    {
        var cancellationDate = new DateTime(2026, 1, 29, 12, 0, 0);
        var cancellationReason = "Zmiana planów";

        var reservation = new Reservation
        {
            CancellationDate = cancellationDate,
            CancellationReason = cancellationReason
        };

        Assert.Equal(cancellationDate, reservation.CancellationDate);
        Assert.Equal(cancellationReason, reservation.CancellationReason);
    }

    [Fact]
    public void Reservation_ShouldAllowSettingUser()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "testuser" };
        var reservation = new Reservation
        {
            UserId = user.Id,
            User = user
        };

        Assert.Equal(user, reservation.User);
        Assert.Equal(user.Id, reservation.UserId);
    }

    [Fact]
    public void Reservation_ShouldAllowSettingVehicle()
    {
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Brand = "BMW" };
        var reservation = new Reservation
        {
            VehicleId = vehicle.Id,
            Vehicle = vehicle
        };

        Assert.Equal(vehicle, reservation.Vehicle);
        Assert.Equal(vehicle.Id, reservation.VehicleId);
    }

    [Fact]
    public void Reservation_ShouldAllowSettingParkingSpace()
    {
        var parkingSpace = new ParkingSpace { Id = Guid.NewGuid(), Code = "A1" };
        var reservation = new Reservation
        {
            ParkingSpaceId = parkingSpace.Id,
            ParkingSpace = parkingSpace
        };

        Assert.Equal(parkingSpace, reservation.ParkingSpace);
        Assert.Equal(parkingSpace.Id, reservation.ParkingSpaceId);
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Confirmed")]
    [InlineData("Cancelled")]
    [InlineData("Completed")]
    public void Reservation_ShouldAcceptDifferentStatuses(string status)
    {
        var reservation = new Reservation { Status = status };

        Assert.Equal(status, reservation.Status);
    }

    [Fact]
    public void Reservation_ShouldHandleDateTimeRanges()
    {
        var bookedFrom = new DateTime(2026, 2, 1, 8, 0, 0);
        var bookedUntil = new DateTime(2026, 2, 1, 18, 0, 0);

        var reservation = new Reservation
        {
            BookedFrom = bookedFrom,
            BookedUntil = bookedUntil
        };

        var duration = reservation.BookedUntil - reservation.BookedFrom;
        Assert.Equal(TimeSpan.FromHours(10), duration);
    }

    [Fact]
    public void Reservation_ShouldHaveAllRequiredRelationships()
    {
        var user = new User { Id = Guid.NewGuid() };
        var vehicle = new Vehicle { Id = Guid.NewGuid() };
        var parkingSpace = new ParkingSpace { Id = Guid.NewGuid() };

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            VehicleId = vehicle.Id,
            Vehicle = vehicle,
            ParkingSpaceId = parkingSpace.Id,
            ParkingSpace = parkingSpace
        };

        Assert.NotNull(reservation.User);
        Assert.NotNull(reservation.Vehicle);
        Assert.NotNull(reservation.ParkingSpace);
    }
}
