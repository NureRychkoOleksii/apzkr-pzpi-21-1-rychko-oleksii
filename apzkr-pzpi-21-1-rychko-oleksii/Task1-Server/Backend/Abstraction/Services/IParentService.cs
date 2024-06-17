using Backend.Core.DTOs;
using Backend.Core.DTOs.Newborn;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface IParentService
{
    Task<IEnumerable<Parent>> GetParents();
    Task<Parent> GetParentAsync(int id);
    Task CreateParentAsync(CreateParentDTO parent);
    Task CreateParentWithNewbornAsync(CreateParentDTO parentDto, CreateNewbornDTO newbornDto);
    Task UpdateParentAsync(int id, UpdateParentDTO parent);
    Task DeleteParentAsync(int id);
    Task<IEnumerable<Newborn>> GetParentNewbornsAsync(int id);
}