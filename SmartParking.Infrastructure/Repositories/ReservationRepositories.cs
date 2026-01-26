using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Infrastructure.Repositories;

public class ReservationRepositories:IReservationRepositories
{
    private readonly SmartParkingDbContext _dbContext;

    public ReservationRepositories(SmartParkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync() => await _dbContext.Reservations.ToListAsync();
    
    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId) => await _dbContext.Reservations.Where(reservation => reservation.UserId == userId).ToListAsync();
    
    public async Task<Reservation?> GetReservationByIdAsync(Guid id) => await _dbContext.Reservations.FirstOrDefaultAsync(reservation => reservation.Id == id);

    public async Task CreateReservationAsync(Reservation reservation)
    {
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateReservationAsync(Reservation reservation)
    {
        _dbContext.Reservations.Update(reservation);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteReservationAsync(Reservation reservation)
    {
        _dbContext.Reservations.Remove(reservation);
        await _dbContext.SaveChangesAsync();
    }
} 