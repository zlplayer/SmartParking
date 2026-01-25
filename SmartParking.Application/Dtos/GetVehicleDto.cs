namespace SmartParking.Application.Dtos;

public class GetVehicleDto
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public string CountryCode { get; set; }
}