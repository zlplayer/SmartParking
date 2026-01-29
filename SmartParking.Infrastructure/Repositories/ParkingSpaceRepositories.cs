using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Infrastructure.Repositories;

public class ParkingSpaceRepositories:IParkingSpaceRepositories
{
    private readonly SmartParkingDbContext _dbContext;

    public ParkingSpaceRepositories(SmartParkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ParkingSpace>> GetAllParkingSpaceAsync() => await _dbContext.ParkingSpaces.ToListAsync();
    public async Task<IEnumerable<ParkingSpace>> GetAllAvailableParkingSpaceAsync() => await _dbContext.ParkingSpaces.Where(x=>x.Status=="Available").ToListAsync();
    
    public async Task<ParkingSpace?> GetParkingSpaceByIdAsync(Guid parkingSpaceId)=> await _dbContext.ParkingSpaces.FirstOrDefaultAsync(x => x.Id == parkingSpaceId);

    public async Task CreateParkingSpaceAsync(ParkingSpace parkingSpace)
    {
        await _dbContext.ParkingSpaces.AddAsync(parkingSpace);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateParkingSpaceAsync(ParkingSpace parkingSpace)
    {
        _dbContext.ParkingSpaces.Update(parkingSpace);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteParkingSpaceAsync(ParkingSpace parkingSpace)
    {
        _dbContext.ParkingSpaces.Remove(parkingSpace);
        await _dbContext.SaveChangesAsync();
    }
}