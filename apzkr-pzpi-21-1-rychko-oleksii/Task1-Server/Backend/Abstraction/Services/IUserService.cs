using Backend.Core.DTOs.User;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserAsync(int id);
    Task CreateUserAsync(CreateUserDTO user);
    Task UpdateUserAsync(int id, UpdateUserDTO user);
    Task DeleteUserAsync(int id);
    public Task<User> AuthenticateUserAsync(string username, string password);
    public Task<bool> SignUpAsync(SignUpDTO signUpDto);
}