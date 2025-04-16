using Backend.DTOs;

namespace Backend.interfaces
{
          public interface IUserService
          {
                    Task<IEnumerable<UserDto>> GetAllUsersAsync();
                    Task<UserDto> GetUserByIdAsync(int id);
                    Task AddUserAsync(UserDto userDto);
                    Task UpdateUserAsync(int id, UserDto userDto);
                    Task DeleteUserAsync(int id);
          }
}