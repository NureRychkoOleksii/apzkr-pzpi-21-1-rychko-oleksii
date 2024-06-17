namespace Backend.Core.Entities;

public class UserParent
{
    public int NewbornId { get; set; }
    public int ParentId { get; set; }
    
    public Newborn Newborn { get; set; }
    public Parent Parent { get; set; }
}