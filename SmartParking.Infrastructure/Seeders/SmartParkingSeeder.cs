using SmartParking.Domain.Entities;

namespace SmartParking.Infrastructure.Seeders;

public class SmartParkingSeeder
{
    private readonly SmartParkingDbContext _dbContext;
    
    public SmartParkingSeeder(SmartParkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }
        }
    }
    
    private IEnumerable<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Admin",
            },
            new Role
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "User",
            },
            new Role
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Technik",
            }
        };
    }
}