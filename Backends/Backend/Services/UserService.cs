using Backend.DTOs;
using Backend.interfaces;
using Backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserService(
        ApplicationDbContext dbContext,
        IMapper mapper) : IUserService
    {

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await dbContext.Users.ToListAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            return mapper.Map<UserDto>(user ?? new User());
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            var user = mapper.Map<User>(userDto);
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int id, UserDto userDto)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser != null)
            {
                mapper.Map(userDto, existingUser);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}