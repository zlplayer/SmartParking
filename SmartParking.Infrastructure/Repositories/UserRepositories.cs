using Microsoft.EntityFrameworkCore;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Infrastructure.Repositories;

public class UserRepositories: IUserRepositories
{
    private readonly SmartParkingDbContext _dbContext;
    
    public UserRepositories(SmartParkingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<User>> GetUsers()=>await _dbContext.Users.ToListAsync();
   
    
    public async Task<User?> GetUser(Guid id)=>await _dbContext.Users.Include(x=>x.Role).FirstOrDefaultAsync(x => x.Id == id);
    
        
    public async Task<User?> GetUserByUsername(string username)=> await _dbContext.Users
        .Include(x => x.Role)
        .FirstOrDefaultAsync(x => x.Username == username);
   
    
    public async Task<User?> GetUserByEmail(string email)=>await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
  

    public async Task CreateUser(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}