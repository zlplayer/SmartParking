using SmartParking.Domain.Entities;

namespace SmartParking.Domain.Interfaces;

public interface IUserRepositories
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUser(Guid id);
    Task<User?> GetUserByEmail(string email);
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(User user);
    Task<User?> GetUserByUsername(string username);
}