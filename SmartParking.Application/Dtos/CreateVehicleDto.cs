namespace SmartParking.Application.Dtos;

public class CreateVehicleDto
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public string CountryCode { get; set; }
    public Guid UserId { get; set; }

}