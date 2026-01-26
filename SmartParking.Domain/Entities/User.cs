namespace SmartParking.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    public Guid RoleId { get; set; } = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public Role Role { get; set; }
    
    public List<Vehicle> Vehicles { get; set; } = new();

    public List<ParkingZone> ParkingZones { get; set; } = new();

    public List<Reservation> Reservations { get; set; } = new();
}