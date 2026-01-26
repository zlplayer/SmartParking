namespace SmartParking.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public string CountryCode { get; set; }
    
    public User User { get; set; }
    public Guid UserId { get; set; }
    
    public List<Reservation> Reservations { get; set; } = new();
}