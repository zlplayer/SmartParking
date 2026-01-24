using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Infrastructure.Repositories;

public class VehicleRepositories:IVehicleRepositories
{
    private readonly SmartParkingDbContext _dbContext;

    public VehicleRepositories(SmartParkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync() => await _dbContext.Vehicles.ToListAsync();
    
    public async Task<Vehicle?> GetVehicleAsync(Guid id) => await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Vehicle>> GetVehiclesByUserIdAsync(Guid userId)
        => await _dbContext.Vehicles.Where(x => x.UserId == userId).ToListAsync();

    
    public async Task CreateVehicleAsync(Vehicle vehicle)
    {
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateVehicleAsync(Vehicle vehicle)
    {
        _dbContext.Vehicles.Update(vehicle);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteVehicleAsync(Vehicle vehicle)
    {
        _dbContext.Vehicles.Remove(vehicle);
        await _dbContext.SaveChangesAsync();
    }
}