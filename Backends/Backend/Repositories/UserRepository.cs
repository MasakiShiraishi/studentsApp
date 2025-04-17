using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.interfaces;
using Backend.Repositories;


namespace Backend.Repositories
{
    public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
    {
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            return await dbContext.SaveChangesAsync() > 0;            
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser != null)
            {
                dbContext.Entry(existingUser).CurrentValues.SetValues(user);
                await dbContext.SaveChangesAsync();
                return existingUser;
            }
            throw new KeyNotFoundException($"User with ID {id} was not found.");
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
          }
}