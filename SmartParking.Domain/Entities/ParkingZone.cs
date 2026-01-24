using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartParking.Domain.Entities;

public class ParkingZone
{ 
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string TariffType  { get; set; }
    public string Regulamin { get; set; }
    public bool IsActive { get; set; }
    
    public List<ParkingSpace> ParkingSpaces { get; set; } = new();

    public Guid? UserId { get; set; }
    public User? User { get; set; }

}