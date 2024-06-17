using Backend.Core.Enums;

namespace Backend.Core.DTOs;

public class CreateParentWithNewbornDto
{
    public int ParentUserId { get; set; }
    public string ParentName { get; set; }
    public string ContractInfo { get; set; }
    public int NewbornUserId { get; set; }
    public string NewbornName { get; set; }
    public DateTime NewbornDateOfBirth { get; set; }
    public Gender Gender { get; set; }
}