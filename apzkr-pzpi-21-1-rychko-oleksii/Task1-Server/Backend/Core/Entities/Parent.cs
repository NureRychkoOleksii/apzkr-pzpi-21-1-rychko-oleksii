using Backend.Core.Enums;

namespace Backend.Core.Entities;

public class Parent: BaseEntity
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string ContractInfo { get; set; }
    
    public User User { get; set; }
    
    public ICollection<UserParent> UserParents;
}