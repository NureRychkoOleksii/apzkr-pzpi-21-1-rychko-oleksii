using Backend.Core.Enums;

namespace Backend.Core.DTOs.User;

public class UpdateUserDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}