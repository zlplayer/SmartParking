namespace SmartParking.Application.Dtos;

public class CreateParkingSpaceDto
{
    public string Code { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public string Status { get; set; }
}