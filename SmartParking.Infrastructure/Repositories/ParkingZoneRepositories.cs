using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Infrastructure.Repositories;

public class ParkingZoneRepositories:IParkingZoneRepositories
{
    private readonly SmartParkingDbContext _dbContext;

    public ParkingZoneRepositories(SmartParkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ParkingZone>> GetAllParkingzonesAsync() => await _dbContext.ParkingZones.ToListAsync();
    
    public async Task<ParkingZone?>  GetParkingZoneAsync(Guid id) => await _dbContext.ParkingZones.FirstOrDefaultAsync(x=>x.Id == id);

    public async Task CreateParkingZoneAsync(ParkingZone parkingZone)
    {
        await _dbContext.ParkingZones.AddAsync(parkingZone);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateParkingZoneAsync(ParkingZone parkingZone)
    {
        _dbContext.ParkingZones.Update(parkingZone);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteParkingZoneAsync(ParkingZone parkingZone)
    {
        _dbContext.ParkingZones.Remove(parkingZone);
        await _dbContext.SaveChangesAsync();
    }

    
}