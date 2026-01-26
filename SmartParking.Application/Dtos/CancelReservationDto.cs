namespace SmartParking.Application.Dtos;

public class CancelReservationDto
{
    public DateTime? CancellationDate { get; set; }
    public string? CancellationReason { get; set; }
}