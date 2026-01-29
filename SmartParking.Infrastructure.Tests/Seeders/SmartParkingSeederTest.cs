using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Infrastructure.Seeders;
using Xunit;

namespace SmartParking.Infrastructure.Tests.Seeders;

[TestSubject(typeof(SmartParkingSeeder))]
public class SmartParkingSeederTest : IDisposable
{
    private readonly SmartParkingDbContext _dbContext;
    private readonly SmartParkingSeeder _seeder;

    public SmartParkingSeederTest()
    {
        var options = new DbContextOptionsBuilder<SmartParkingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new SmartParkingDbContext(options);
        _seeder = new SmartParkingSeeder(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public void Seed_WhenDatabaseIsEmpty_ShouldSeedRoles()
    {
        // Act
        _seeder.Seed();

        // Assert
        var roles = _dbContext.Roles.ToList();
        Assert.Equal(3, roles.Count);
    }

    [Fact]
    public void Seed_ShouldCreateAdminRole()
    {
        // Act
        _seeder.Seed();

        // Assert
        var adminRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
        Assert.NotNull(adminRole);
        Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), adminRole.Id);
    }

    [Fact]
    public void Seed_ShouldCreateUserRole()
    {
        // Act
        _seeder.Seed();

        // Assert
        var userRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "User");
        Assert.NotNull(userRole);
        Assert.Equal(Guid.Parse("22222222-2222-2222-2222-222222222222"), userRole.Id);
    }

    [Fact]
    public void Seed_ShouldCreateTechnikRole()
    {
        // Act
        _seeder.Seed();

        // Assert
        var technikRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "Technik");
        Assert.NotNull(technikRole);
        Assert.Equal(Guid.Parse("33333333-3333-3333-3333-333333333333"), technikRole.Id);
    }

    [Fact]
    public void Seed_WhenRolesAlreadyExist_ShouldNotDuplicateRoles()
    {
        // Arrange
        var existingRole = new Role
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Admin"
        };
        _dbContext.Roles.Add(existingRole);
        _dbContext.SaveChanges();

        // Act
        _seeder.Seed();

        // Assert
        var roles = _dbContext.Roles.ToList();
        Assert.Single(roles);
    }

    [Fact]
    public void Seed_ShouldBeIdempotent()
    {
        // Act
        _seeder.Seed();
        _seeder.Seed();
        _seeder.Seed();

        // Assert
        var roles = _dbContext.Roles.ToList();
        Assert.Equal(3, roles.Count);
    }

    [Fact]
    public void Seed_AllRolesShouldHaveCorrectNames()
    {
        // Act
        _seeder.Seed();

        // Assert
        var roleNames = _dbContext.Roles.Select(r => r.Name).ToList();
        Assert.Contains("Admin", roleNames);
        Assert.Contains("User", roleNames);
        Assert.Contains("Technik", roleNames);
    }

    [Fact]
    public void Seed_AllRolesShouldHaveUniqueIds()
    {
        // Act
        _seeder.Seed();

        // Assert
        var roleIds = _dbContext.Roles.Select(r => r.Id).ToList();
        Assert.Equal(3, roleIds.Distinct().Count());
    }

    [Fact]
    public void Seed_RoleIdsShouldMatchExpectedValues()
    {
        // Act
        _seeder.Seed();

        // Assert
        var expectedIds = new[]
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("33333333-3333-3333-3333-333333333333")
        };

        var actualIds = _dbContext.Roles.Select(r => r.Id).ToList();

        foreach (var expectedId in expectedIds)
        {
            Assert.Contains(expectedId, actualIds);
        }
    }
}
