using Backend.Core.Enums;

namespace Backend.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    
    public ICollection<Newborn> Patients { get; set; }
}