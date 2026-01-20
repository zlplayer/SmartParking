using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;

namespace SmartParking.Infrastructure;

public class SmartParkingDbContext : DbContext
{
    public DbSet<User>  Users { get; set; }
    public DbSet<Role>  Roles { get; set; }
    
    public SmartParkingDbContext(DbContextOptions<SmartParkingDbContext> options):base(options)
    {
        
    }
    
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    
}