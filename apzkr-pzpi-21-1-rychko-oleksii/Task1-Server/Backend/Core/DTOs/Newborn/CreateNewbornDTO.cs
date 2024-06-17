using Backend.Core.Enums;

namespace Backend.Core.DTOs.Newborn;

public class CreateNewbornDTO
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
}