using System.Security.Cryptography;
using System.Text;
using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.User;
using Backend.Core.Entities;
using Backend.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class UserService : IUserService
{
    private readonly StarOfLifeContext _context;

    public UserService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task CreateUserAsync(CreateUserDTO user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        var (hashedPassword, salt) = HashPassword(user.Password);

        try
        {
            await _context.Users.AddAsync(new User
            {
                Role = user.Role,
                Email = user.Email,
                Password = hashedPassword,
                Salt = salt,
                Username = user.Username
            });

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new BadHttpRequestException(e.Message);
        }
    }

    public async Task UpdateUserAsync(int id, UpdateUserDTO user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userDb = await _context.FindAsync<User>(id);
        
        if (userDb == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        userDb.Email = user.Email;
        userDb.Role = user.Role;
        userDb.Username = user.Username;
        
        var (hashedPassword, salt) = HashPassword(user.Password);

        userDb.Password = hashedPassword;
        userDb.Salt = salt;
        
        _context.Users.Update(userDb);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> SignUpAsync(SignUpDTO signUpDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == signUpDto.Email);

        if (existingUser != null)
        {
            return false;
        }
        
        var (hashedPassword, salt) = HashPassword(signUpDto.Password);
        
        var newUser = new User
        {
            Role = Role.Parent,
            Email = signUpDto.Email,
            Password = hashedPassword,
            Salt = salt,
            Username = signUpDto.Username,
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return true;
    }
    
    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (user != null && IsPasswordValid(password, user.Password, user.Salt))
        {
            return user;
        }

        return null;
    }
    
    private (string hashedPassword, string salt) HashPassword(string password)
    {
        var saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        
        var combinedBytes = Encoding.UTF8.GetBytes(password).Concat(saltBytes).ToArray();
        using (var sha256 = new SHA256Managed())
        {
            var hashedBytes = sha256.ComputeHash(combinedBytes);
            var hashedPassword = Convert.ToBase64String(hashedBytes);

            return (hashedPassword, Convert.ToBase64String(saltBytes));
        }
    }
    
    private bool IsPasswordValid(string enteredPassword, string storedPassword, string salt)
    {
        byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] saltedPasswordBytes = new byte[enteredPasswordBytes.Length + saltBytes.Length];
        Array.Copy(enteredPasswordBytes, saltedPasswordBytes, enteredPasswordBytes.Length);
        Array.Copy(saltBytes, 0, saltedPasswordBytes, enteredPasswordBytes.Length, saltBytes.Length);

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(saltedPasswordBytes);
            string enteredHash = Convert.ToBase64String(hashedBytes);
            
            return string.Equals(enteredHash, storedPassword);
        }
    }
}