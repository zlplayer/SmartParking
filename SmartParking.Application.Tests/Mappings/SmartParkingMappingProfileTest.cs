using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using SmartParking.Application.Dtos;
using SmartParking.Application.Mappings;
using SmartParking.Application.SmartParking.Commands.CancelReservation;
using SmartParking.Application.SmartParking.Commands.CreateParkingSpace;
using SmartParking.Application.SmartParking.Commands.CreateParkingZone;
using SmartParking.Application.SmartParking.Commands.CreateReservation;
using SmartParking.Application.SmartParking.Commands.CreateVehicle;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;
using SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;
using SmartParking.Application.SmartParking.Commands.UpdateParkingZone;
using SmartParking.Application.SmartParking.Commands.UpdateReservation;
using SmartParking.Application.SmartParking.Commands.UpdateVehicle;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Application.Tests.Mappings;

[TestSubject(typeof(SmartParkingMappingProfile))]
public class SmartParkingMappingProfileTest
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _config;

    public SmartParkingMappingProfileTest()
    {
        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<SmartParkingMappingProfile>();
        _config = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
        _mapper = _config.CreateMapper();
    }

    #region User Mappings

    [Fact]
    public void Map_UserToUserDto_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            Role = new Role { Name = "Admin" }
        };

        // Act
        var result = _mapper.Map<UserDto>(user);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public void Map_RegisterCommandToUser_ShouldMapPasswordToPasswordHash()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "new@example.com",
            Username = "newuser",
            Password = "hashedPassword123",
            FirstName = "New",
            LastName = "User"
        };

        // Act
        var result = _mapper.Map<User>(command);

        // Assert
        Assert.Equal(command.Email, result.Email);
        Assert.Equal(command.Username, result.Username);
        Assert.Equal(command.Password, result.PasswordHash);
        Assert.Equal(command.FirstName, result.FirstName);
        Assert.Equal(command.LastName, result.LastName);
    }

    #endregion

    #region ParkingZone Mappings

    [Fact]
    public void Map_CreateParkingZoneCommandToParkingZone_ShouldMapCorrectly()
    {
        // Arrange
        var command = new CreateParkingZoneCommand
        {
            Name = "Zone A",
            City = "Warsaw",
            TariffType = "Standard",
            Regulamin = "Rules apply",
            IsActive = true,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.Map<ParkingZone>(command);

        // Assert
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.City, result.City);
        Assert.Equal(command.TariffType, result.TariffType);
        Assert.Equal(command.Regulamin, result.Regulamin);
        Assert.Equal(command.IsActive, result.IsActive);
        Assert.Equal(command.UserId, result.UserId);
    }

    [Fact]
    public void Map_ParkingZoneToGetParkingZoneDto_ShouldMapCorrectly()
    {
        // Arrange
        var parkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "Zone B",
            City = "Krakow",
            TariffType = "Premium",
            Regulamin = "Premium rules",
            IsActive = true,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.Map<GetParkingZoneDto>(parkingZone);

        // Assert
        Assert.Equal(parkingZone.Id, result.Id);
        Assert.Equal(parkingZone.Name, result.Name);
        Assert.Equal(parkingZone.City, result.City);
        Assert.Equal(parkingZone.TariffType, result.TariffType);
        Assert.Equal(parkingZone.Regulamin, result.Regulamin);
        Assert.Equal(parkingZone.IsActive, result.IsActive);
        Assert.Equal(parkingZone.UserId, result.UserId);
    }

    [Fact]
    public void Map_UpdateParkingZoneCommandToParkingZone_ShouldMapCorrectly()
    {
        // Arrange
        var existingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            Name = "Original",
            City = "Original City"
        };

        var command = new UpdateParkingZoneCommand
        {
            Name = "Updated Zone",
            City = "Updated City",
            TariffType = "Premium",
            Regulamin = "New rules",
            IsActive = false
        };

        // Act
        _mapper.Map(command, existingZone);

        // Assert
        Assert.Equal(command.Name, existingZone.Name);
        Assert.Equal(command.City, existingZone.City);
        Assert.Equal(command.TariffType, existingZone.TariffType);
        Assert.Equal(command.Regulamin, existingZone.Regulamin);
        Assert.Equal(command.IsActive, existingZone.IsActive);
    }

    #endregion

    #region Vehicle Mappings

    [Fact]
    public void Map_CreateVehicleCommandToVehicle_ShouldMapCorrectly()
    {
        // Arrange
        var command = new CreateVehicleCommand
        {
            Brand = "Toyota",
            Model = "Corolla",
            RegistrationNumber = "WA12345",
            CountryCode = "PL",
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.Map<Vehicle>(command);

        // Assert
        Assert.Equal(command.Brand, result.Brand);
        Assert.Equal(command.Model, result.Model);
        Assert.Equal(command.RegistrationNumber, result.RegistrationNumber);
        Assert.Equal(command.CountryCode, result.CountryCode);
        Assert.Equal(command.UserId, result.UserId);
    }

    [Fact]
    public void Map_VehicleToGetVehicleDto_ShouldMapCorrectly()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "Honda",
            Model = "Civic",
            RegistrationNumber = "KR54321",
            CountryCode = "PL"
        };

        // Act
        var result = _mapper.Map<GetVehicleDto>(vehicle);

        // Assert
        Assert.Equal(vehicle.Id, result.Id);
        Assert.Equal(vehicle.Brand, result.Brand);
        Assert.Equal(vehicle.Model, result.Model);
        Assert.Equal(vehicle.RegistrationNumber, result.RegistrationNumber);
        Assert.Equal(vehicle.CountryCode, result.CountryCode);
    }

    [Fact]
    public void Map_UpdateVehicleCommandToVehicle_ShouldMapCorrectly()
    {
        // Arrange
        var existingVehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = "Original",
            Model = "Original Model"
        };

        var command = new UpdateVehicleCommand
        {
            Brand = "Updated Brand",
            Model = "Updated Model",
            RegistrationNumber = "NEW1234",
            CountryCode = "DE"
        };

        // Act
        _mapper.Map(command, existingVehicle);

        // Assert
        Assert.Equal(command.Brand, existingVehicle.Brand);
        Assert.Equal(command.Model, existingVehicle.Model);
        Assert.Equal(command.RegistrationNumber, existingVehicle.RegistrationNumber);
        Assert.Equal(command.CountryCode, existingVehicle.CountryCode);
    }

    #endregion

    #region ParkingSpace Mappings

    [Fact]
    public void Map_CreateParkingSpaceCommandToParkingSpace_ShouldMapCorrectly()
    {
        // Arrange
        var command = new CreateParkingSpaceCommand
        {
            Code = "A1",
            Width = 2.5f,
            Height = 5.0f,
            Status = "Available",
            ParkingZoneId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.Map<ParkingSpace>(command);

        // Assert
        Assert.Equal(command.Code, result.Code);
        Assert.Equal(command.Width, result.Width);
        Assert.Equal(command.Height, result.Height);
        Assert.Equal(command.Status, result.Status);
        Assert.Equal(command.ParkingZoneId, result.ParkingZoneId);
    }

    [Fact]
    public void Map_ParkingSpaceToGetParkingSpaceDto_ShouldMapCorrectly()
    {
        // Arrange
        var parkingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = "B2",
            Width = 3.0f,
            Height = 6.0f,
            Status = "Reserved"
        };

        // Act
        var result = _mapper.Map<GetParkingSpaceDto>(parkingSpace);

        // Assert
        Assert.Equal(parkingSpace.Id, result.Id);
        Assert.Equal(parkingSpace.Code, result.Code);
        Assert.Equal(parkingSpace.Width, result.Width);
        Assert.Equal(parkingSpace.Height, result.Height);
        Assert.Equal(parkingSpace.Status, result.Status);
    }

    [Fact]
    public void Map_UpdateParkingSpaceCommandToParkingSpace_ShouldMapCorrectly()
    {
        // Arrange
        var existingSpace = new ParkingSpace
        {
            Id = Guid.NewGuid(),
            Code = "A1",
            Width = 2.5f,
            Height = 5.0f,
            Status = "Available"
        };

        var command = new UpdateParkingSpaceCommand
        {
            Code = "A1-Updated",
            Width = 3.0f,
            Height = 6.0f,
            Status = "Reserved"
        };

        // Act
        _mapper.Map(command, existingSpace);

        // Assert
        Assert.Equal(command.Code, existingSpace.Code);
        Assert.Equal(command.Width, existingSpace.Width);
        Assert.Equal(command.Height, existingSpace.Height);
        Assert.Equal(command.Status, existingSpace.Status);
    }

    #endregion

    #region Reservation Mappings

    [Fact]
    public void Map_CreateReservationCommandToReservation_ShouldMapCorrectly()
    {
        // Arrange
        var command = new CreateReservationCommand
        {
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            Status = "Pending",
            CreationDate = DateTime.UtcNow,
            ConfirmationDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.Map<Reservation>(command);

        // Assert
        Assert.Equal(command.BookedFrom, result.BookedFrom);
        Assert.Equal(command.BookedUntil, result.BookedUntil);
        Assert.Equal(command.Status, result.Status);
        Assert.Equal(command.CreationDate, result.CreationDate);
        Assert.Equal(command.ConfirmationDate, result.ConfirmationDate);
        Assert.Equal(command.UserId, result.UserId);
        Assert.Equal(command.VehicleId, result.VehicleId);
        Assert.Equal(command.ParkingSpaceId, result.ParkingSpaceId);
    }

    [Fact]
    public void Map_ReservationToGetReservationDto_ShouldMapCorrectly()
    {
        // Arrange
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            BookedFrom = DateTime.UtcNow,
            BookedUntil = DateTime.UtcNow.AddHours(2),
            Status = "Active",
            CreationDate = DateTime.UtcNow.AddDays(-1),
            ConfirmationDate = DateTime.UtcNow.AddDays(-1),
            CancellationDate = null,
            CancellationReason = null,
            UserId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ParkingSpaceId = Guid.NewGuid()
        };

        // Act
        var result = _mapper.Map<GetReservationDto>(reservation);

        // Assert
        Assert.Equal(reservation.Id, result.Id);
        Assert.Equal(reservation.BookedFrom, result.BookedFrom);
        Assert.Equal(reservation.BookedUntil, result.BookedUntil);
        Assert.Equal(reservation.Status, result.Status);
        Assert.Equal(reservation.CreationDate, result.CreationDate);
        Assert.Equal(reservation.ConfirmationDate, result.ConfirmationDate);
        Assert.Equal(reservation.CancellationDate, result.CancellationDate);
        Assert.Equal(reservation.CancellationReason, result.CancellationReason);
        Assert.Equal(reservation.UserId, result.UserId);
        Assert.Equal(reservation.VehicleId, result.VehicleId);
        Assert.Equal(reservation.ParkingSpaceId, result.ParkingSpaceId);
    }

    [Fact]
    public void Map_CancelReservationCommandToReservation_ShouldMapCancellationFields()
    {
        // Arrange
        var existingReservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Active",
            CancellationDate = null,
            CancellationReason = null
        };

        var cancellationDate = DateTime.UtcNow;
        var command = new CancelReservationCommand
        {
            CancellationDate = cancellationDate,
            CancellationReason = "User requested cancellation"
        };

        // Act
        _mapper.Map(command, existingReservation);

        // Assert
        Assert.Equal(cancellationDate, existingReservation.CancellationDate);
        Assert.Equal("User requested cancellation", existingReservation.CancellationReason);
    }

    [Fact]
    public void Map_UpdateReservationCommandToReservation_ShouldMapCorrectly()
    {
        // Arrange
        var existingReservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = "Pending"
        };

        var command = new UpdateReservationCommand
        {
            BookedFrom = DateTime.UtcNow.AddDays(1),
            BookedUntil = DateTime.UtcNow.AddDays(1).AddHours(3),
            Status = "Confirmed"
        };

        // Act
        _mapper.Map(command, existingReservation);

        // Assert
        Assert.Equal(command.BookedFrom, existingReservation.BookedFrom);
        Assert.Equal(command.BookedUntil, existingReservation.BookedUntil);
        Assert.Equal(command.Status, existingReservation.Status);
    }

    #endregion

    #region Collection Mappings

    [Fact]
    public void Map_ParkingZoneCollectionToGetParkingZoneDtoCollection_ShouldMapAllItems()
    {
        // Arrange
        var parkingZones = new List<ParkingZone>
        {
            new() { Id = Guid.NewGuid(), Name = "Zone 1", City = "City 1" },
            new() { Id = Guid.NewGuid(), Name = "Zone 2", City = "City 2" },
            new() { Id = Guid.NewGuid(), Name = "Zone 3", City = "City 3" }
        };

        // Act
        var result = _mapper.Map<IEnumerable<GetParkingZoneDto>>(parkingZones);

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void Map_VehicleCollectionToGetVehicleDtoCollection_ShouldMapAllItems()
    {
        // Arrange
        var vehicles = new List<Vehicle>
        {
            new() { Id = Guid.NewGuid(), Brand = "Toyota", Model = "Corolla" },
            new() { Id = Guid.NewGuid(), Brand = "Honda", Model = "Civic" }
        };

        // Act
        var result = _mapper.Map<IEnumerable<GetVehicleDto>>(vehicles);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void Map_ReservationCollectionToGetReservationDtoCollection_ShouldMapAllItems()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new() { Id = Guid.NewGuid(), Status = "Active" },
            new() { Id = Guid.NewGuid(), Status = "Completed" },
            new() { Id = Guid.NewGuid(), Status = "Cancelled" }
        };

        // Act
        var result = _mapper.Map<IEnumerable<GetReservationDto>>(reservations);

        // Assert
        Assert.Equal(3, result.Count());
    }

    #endregion
}
