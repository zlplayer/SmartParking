using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Infrastructure.Repositories;
using Xunit;

namespace SmartParking.Infrastructure.Tests.Repositories;

[TestSubject(typeof(UserRepositories))]
public class UserRepositoriesTest : IDisposable
{
    private readonly SmartParkingDbContext _dbContext;
    private readonly UserRepositories _repository;

    public UserRepositoriesTest()
    {
        var options = new DbContextOptionsBuilder<SmartParkingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new SmartParkingDbContext(options);
        _repository = new UserRepositories(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private User CreateTestUser(string username = "testuser", string email = "test@test.com")
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hashedpassword123"
        };
    }

    #region GetUsers

    [Fact]
    public async Task GetUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUser("user1", "user1@test.com"),
            CreateTestUser("user2", "user2@test.com")
        };
        await _dbContext.Users.AddRangeAsync(users);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetUsers();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetUsers_WhenNoUsers_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _repository.GetUsers();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetUser

    [Fact]
    public async Task GetUser_WhenUserExists_ShouldReturnUserWithRole()
    {
        // Arrange
        var role = new Role { Id = Guid.NewGuid(), Name = "Admin" };
        var user = CreateTestUser();
        user.RoleId = role.Id;
        user.Role = role;

        await _dbContext.Roles.AddAsync(role);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetUser(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Username, result.Username);
        Assert.NotNull(result.Role);
        Assert.Equal("Admin", result.Role.Name);
    }

    [Fact]
    public async Task GetUser_WhenUserNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetUser(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetUserByUsername

    [Fact]
    public async Task GetUserByUsername_WhenUserExists_ShouldReturnUserWithRole()
    {
        // Arrange
        var role = new Role { Id = Guid.NewGuid(), Name = "User" };
        var user = CreateTestUser("findme", "findme@test.com");
        user.RoleId = role.Id;
        user.Role = role;

        await _dbContext.Roles.AddAsync(role);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserByUsername("findme");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("findme", result.Username);
        Assert.NotNull(result.Role);
    }

    [Fact]
    public async Task GetUserByUsername_WhenUserNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetUserByUsername("nonexistent");

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetUserByEmail

    [Fact]
    public async Task GetUserByEmail_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user = CreateTestUser("testuser", "unique@test.com");
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserByEmail("unique@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("unique@test.com", result.Email);
    }

    [Fact]
    public async Task GetUserByEmail_WhenUserNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetUserByEmail("notfound@test.com");

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region CreateUser

    [Fact]
    public async Task CreateUser_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = CreateTestUser("newuser", "new@test.com");

        // Act
        await _repository.CreateUser(user);

        // Assert
        var savedUser = await _dbContext.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("newuser", savedUser.Username);
        Assert.Equal("new@test.com", savedUser.Email);
    }

    #endregion

    #region UpdateUser

    [Fact]
    public async Task UpdateUser_ShouldUpdateUserInDatabase()
    {
        // Arrange
        var user = CreateTestUser("original", "original@test.com");
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(user).State = EntityState.Detached;

        // Act
        user.Username = "updated";
        user.Email = "updated@test.com";
        await _repository.UpdateUser(user);

        // Assert
        var updatedUser = await _dbContext.Users.FindAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("updated", updatedUser.Username);
        Assert.Equal("updated@test.com", updatedUser.Email);
    }

    #endregion

    #region DeleteUser

    [Fact]
    public async Task DeleteUser_ShouldRemoveUserFromDatabase()
    {
        // Arrange
        var user = CreateTestUser("todelete", "delete@test.com");
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteUser(user);

        // Assert
        var deletedUser = await _dbContext.Users.FindAsync(user.Id);
        Assert.Null(deletedUser);
    }

    #endregion
}
