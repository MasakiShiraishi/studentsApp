using Backend.Models; 

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
}