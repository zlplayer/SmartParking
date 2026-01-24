using SmartParking.Domain.Entities;

namespace SmartParking.Domain.Interfaces;

public interface IVehicleRepositories
{
    Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
    Task<Vehicle?> GetVehicleAsync(Guid id);
    Task<IEnumerable<Vehicle>> GetVehiclesByUserIdAsync(Guid userId);
    Task CreateVehicleAsync(Vehicle vehicle);
    Task UpdateVehicleAsync(Vehicle vehicle);
    Task DeleteVehicleAsync(Vehicle vehicle);
}