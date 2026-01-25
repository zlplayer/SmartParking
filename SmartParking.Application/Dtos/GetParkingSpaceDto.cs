namespace SmartParking.Application.Dtos;

public class GetParkingSpaceDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public string Status { get; set; }
}