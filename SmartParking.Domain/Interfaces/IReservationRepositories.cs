using SmartParking.Domain.Entities;

namespace SmartParking.Domain.Interfaces;

public interface IReservationRepositories
{
    Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId);
    Task<Reservation?> GetReservationByIdAsync(Guid id);
    Task CreateReservationAsync(Reservation reservation);
    Task UpdateReservationAsync(Reservation reservation);
    Task DeleteReservationAsync(Reservation reservation);
}