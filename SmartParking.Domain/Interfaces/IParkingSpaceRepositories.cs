using SmartParking.Domain.Entities;

namespace SmartParking.Domain.Interfaces;

public interface IParkingSpaceRepositories
{
    Task<IEnumerable<ParkingSpace>> GetAllParkingSpaceAsync();
    Task<ParkingSpace?> GetParkingSpaceByIdAsync(Guid parkingSpaceId);
    Task<IEnumerable<ParkingSpace>> GetAllAvailableParkingSpaceAsync();
    Task CreateParkingSpaceAsync(ParkingSpace parkingSpace);
    Task UpdateParkingSpaceAsync(ParkingSpace parkingSpace);
    Task DeleteParkingSpaceAsync(ParkingSpace parkingSpace);
}