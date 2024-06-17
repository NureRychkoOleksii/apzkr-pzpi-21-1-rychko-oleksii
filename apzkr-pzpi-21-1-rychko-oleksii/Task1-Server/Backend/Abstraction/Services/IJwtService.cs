using Backend.Core.Enums;

namespace Backend.Abstraction.Services;

public interface IJwtService
{
    public string GenerateToken(int userId, Role role);
}