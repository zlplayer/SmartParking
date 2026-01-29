using System;
using JetBrains.Annotations;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Domain.Tests.Entities;

[TestSubject(typeof(ParkingSpace))]
public class ParkingSpaceTest
{
    [Fact]
    public void ParkingSpace_ShouldCreateInstance()
    {
        var parkingSpace = new ParkingSpace();

        Assert.NotNull(parkingSpace);
    }

    [Fact]
    public void ParkingSpace_ShouldSetAndGetProperties()
    {
        var id = Guid.NewGuid();
        var parkingZoneId = Guid.NewGuid();

        var parkingSpace = new ParkingSpace
        {
            Id = id,
            Code = "A1",
            Width = 2.5f,
            Height = 5.0f,
            Status = "Available",
            ParkingZoneId = parkingZoneId
        };

        Assert.Equal(id, parkingSpace.Id);
        Assert.Equal("A1", parkingSpace.Code);
        Assert.Equal(2.5f, parkingSpace.Width);
        Assert.Equal(5.0f, parkingSpace.Height);
        Assert.Equal("Available", parkingSpace.Status);
        Assert.Equal(parkingZoneId, parkingSpace.ParkingZoneId);
    }

    [Fact]
    public void ParkingSpace_ReservationsList_ShouldBeInitializedAsEmptyList()
    {
        var parkingSpace = new ParkingSpace();

        Assert.NotNull(parkingSpace.Reservations);
        Assert.Empty(parkingSpace.Reservations);
    }

    [Fact]
    public void ParkingSpace_ShouldAllowAddingReservations()
    {
        var parkingSpace = new ParkingSpace();
        var reservation = new Reservation { Id = Guid.NewGuid() };

        parkingSpace.Reservations.Add(reservation);

        Assert.Single(parkingSpace.Reservations);
        Assert.Contains(reservation, parkingSpace.Reservations);
    }

    [Fact]
    public void ParkingSpace_ShouldAllowSettingParkingZone()
    {
        var parkingZone = new ParkingZone { Id = Guid.NewGuid(), Name = "Strefa A" };
        var parkingSpace = new ParkingSpace
        {
            ParkingZoneId = parkingZone.Id,
            ParkingZone = parkingZone
        };

        Assert.Equal(parkingZone, parkingSpace.ParkingZone);
        Assert.Equal(parkingZone.Id, parkingSpace.ParkingZoneId);
    }

    [Theory]
    [InlineData("Available")]
    [InlineData("Occupied")]
    [InlineData("Reserved")]
    [InlineData("OutOfService")]
    public void ParkingSpace_ShouldAcceptDifferentStatuses(string status)
    {
        var parkingSpace = new ParkingSpace { Status = status };

        Assert.Equal(status, parkingSpace.Status);
    }

    [Fact]
    public void ParkingSpace_ShouldHandleFloatDimensions()
    {
        var parkingSpace = new ParkingSpace
        {
            Width = 2.75f,
            Height = 5.25f
        };

        Assert.Equal(2.75f, parkingSpace.Width);
        Assert.Equal(5.25f, parkingSpace.Height);
    }

    [Fact]
    public void ParkingSpace_ShouldAllowMultipleReservations()
    {
        var parkingSpace = new ParkingSpace();
        var reservation1 = new Reservation { Id = Guid.NewGuid() };
        var reservation2 = new Reservation { Id = Guid.NewGuid() };
        var reservation3 = new Reservation { Id = Guid.NewGuid() };

        parkingSpace.Reservations.Add(reservation1);
        parkingSpace.Reservations.Add(reservation2);
        parkingSpace.Reservations.Add(reservation3);

        Assert.Equal(3, parkingSpace.Reservations.Count);
    }
}
