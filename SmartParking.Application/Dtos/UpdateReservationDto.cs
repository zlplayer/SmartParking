namespace SmartParking.Application.Dtos;

public class UpdateReservationDto
{
    public DateTime BookedFrom { get; set; }
    public DateTime BookedUntil { get; set; }
    public string Status { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid ParkingSpaceId { get; set; }
}