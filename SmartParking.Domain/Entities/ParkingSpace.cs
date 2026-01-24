using System.ComponentModel.DataAnnotations.Schema;

namespace SmartParking.Domain.Entities;

public class ParkingSpace
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public string Status { get; set; }
    
    public Guid ParkingZoneId { get; set; }
    public ParkingZone ParkingZone { get; set; }
}