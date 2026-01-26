namespace SmartParking.Application.Dtos;

public class UpdateParkingSpaceDto
{
    public string Code { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public string Status { get; set; }
}