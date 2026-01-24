using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;

namespace SmartParking.Infrastructure;

public class SmartParkingDbContext : DbContext
{
    public DbSet<User>  Users { get; set; }
    public DbSet<Role>  Roles { get; set; }
    public DbSet<Vehicle>  Vehicles { get; set; }
    public DbSet<ParkingZone> ParkingZones { get; set; }
    public DbSet<ParkingSpace> ParkingSpaces { get; set; }
    
    public SmartParkingDbContext(DbContextOptions<SmartParkingDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingZone>().HasMany(x=>x.ParkingSpaces).WithOne(x=>x.ParkingZone);
        
        modelBuilder.Entity<User>().HasMany(x=>x.ParkingZones).WithOne(x=>x.User);
        modelBuilder.Entity<User>().HasMany(x=>x.Vehicles).WithOne(x=>x.User);

    }
 
    
}