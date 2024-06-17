using Backend.Core.Enums;

namespace Backend.Core.Entities;

public class Newborn : BaseEntity
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    
    public User User { get; set; }

    public ICollection<UserParent> UserParents;
}