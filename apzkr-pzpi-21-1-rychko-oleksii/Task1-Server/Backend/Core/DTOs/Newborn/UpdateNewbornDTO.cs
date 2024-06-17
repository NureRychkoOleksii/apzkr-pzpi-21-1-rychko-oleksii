using Backend.Core.Enums;

namespace Backend.Core.DTOs.Newborn;

public class UpdateNewbornDTO
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
}