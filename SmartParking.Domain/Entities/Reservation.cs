namespace SmartParking.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public DateTime BookedFrom { get; set; }
    public DateTime BookedUntil { get; set; }
    public string Status { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? CancellationReason { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public Guid ParkingSpaceId { get; set; }
    public ParkingSpace ParkingSpace { get; set; }
}