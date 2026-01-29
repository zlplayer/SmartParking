using System;
using JetBrains.Annotations;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Domain.Tests.Entities;

[TestSubject(typeof(ParkingZone))]
public class ParkingZoneTest
{
    [Fact]
    public void ParkingZone_ShouldCreateInstance()
    {
        var parkingZone = new ParkingZone();

        Assert.NotNull(parkingZone);
    }

    [Fact]
    public void ParkingZone_ShouldSetAndGetProperties()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var parkingZone = new ParkingZone
        {
            Id = id,
            Name = "Strefa A",
            City = "Warszawa",
            TariffType = "Hourly",
            Regulamin = "Regulamin strefy A",
            IsActive = true,
            UserId = userId
        };

        Assert.Equal(id, parkingZone.Id);
        Assert.Equal("Strefa A", parkingZone.Name);
        Assert.Equal("Warszawa", parkingZone.City);
        Assert.Equal("Hourly", parkingZone.TariffType);
        Assert.Equal("Regulamin strefy A", parkingZone.Regulamin);
        Assert.True(parkingZone.IsActive);
        Assert.Equal(userId, parkingZone.UserId);
    }

    [Fact]
    public void ParkingZone_UserId_ShouldBeNullable()
    {
        var parkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "Strefa B",
            UserId = null
        };

        Assert.Null(parkingZone.UserId);
    }

    [Fact]
    public void ParkingZone_ParkingSpacesList_ShouldBeInitializedAsEmptyList()
    {
        var parkingZone = new ParkingZone();

        Assert.NotNull(parkingZone.ParkingSpaces);
        Assert.Empty(parkingZone.ParkingSpaces);
    }

    [Fact]
    public void ParkingZone_ShouldAllowAddingParkingSpaces()
    {
        var parkingZone = new ParkingZone();
        var parkingSpace = new ParkingSpace { Id = Guid.NewGuid(), Code = "A1" };

        parkingZone.ParkingSpaces.Add(parkingSpace);

        Assert.Single(parkingZone.ParkingSpaces);
        Assert.Contains(parkingSpace, parkingZone.ParkingSpaces);
    }

    [Fact]
    public void ParkingZone_ShouldAllowSettingUser()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "owner" };
        var parkingZone = new ParkingZone
        {
            UserId = user.Id,
            User = user
        };

        Assert.Equal(user, parkingZone.User);
        Assert.Equal(user.Id, parkingZone.UserId);
    }

    [Fact]
    public void ParkingZone_IsActive_ShouldDefaultToFalse()
    {
        var parkingZone = new ParkingZone();

        Assert.False(parkingZone.IsActive);
    }

    [Theory]
    [InlineData("Hourly")]
    [InlineData("Daily")]
    [InlineData("Monthly")]
    public void ParkingZone_ShouldAcceptDifferentTariffTypes(string tariffType)
    {
        var parkingZone = new ParkingZone { TariffType = tariffType };

        Assert.Equal(tariffType, parkingZone.TariffType);
    }
}
