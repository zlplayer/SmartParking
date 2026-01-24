namespace SmartParking.Application.Dtos;

public class GetParkingZoneDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string TariffType  { get; set; }
    public string Regulamin { get; set; }
    public bool IsActive { get; set; }
    public Guid? UserId { get; set; }

}