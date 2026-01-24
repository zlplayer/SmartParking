using SmartParking.Domain.Entities;

namespace SmartParking.Domain.Interfaces;

public interface IParkingZoneRepositories
{
    Task<IEnumerable<ParkingZone>> GetAllParkingzonesAsync();
    Task<ParkingZone?> GetParkingZoneAsync(Guid id);
    Task CreateParkingZoneAsync(ParkingZone parkingZone);
    Task UpdateParkingZoneAsync(ParkingZone parkingZone);
    Task DeleteParkingZoneAsync(ParkingZone parkingZone);
}