using System;
using JetBrains.Annotations;
using SmartParking.Domain.Entities;
using Xunit;

namespace SmartParking.Domain.Tests.Entities;

[TestSubject(typeof(Role))]
public class RoleTest
{
    [Fact]
    public void Role_ShouldCreateInstance()
    {
        var role = new Role();

        Assert.NotNull(role);
    }

    [Fact]
    public void Role_ShouldSetAndGetProperties()
    {
        var id = Guid.NewGuid();

        var role = new Role
        {
            Id = id,
            Name = "Admin"
        };

        Assert.Equal(id, role.Id);
        Assert.Equal("Admin", role.Name);
    }

    [Fact]
    public void Role_ShouldAllowDifferentRoleNames()
    {
        var userRole = new Role { Id = Guid.NewGuid(), Name = "User" };
        var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin" };
        var moderatorRole = new Role { Id = Guid.NewGuid(), Name = "Moderator" };

        Assert.Equal("User", userRole.Name);
        Assert.Equal("Admin", adminRole.Name);
        Assert.Equal("Moderator", moderatorRole.Name);
    }
}
