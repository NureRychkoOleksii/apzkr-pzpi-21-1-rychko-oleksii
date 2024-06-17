using Backend.Core.DTOs.Newborn;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface INewbornService
{
    Task<IEnumerable<Newborn>> GetNewbornsAsync();
    Task<Newborn> GetNewbornAsync(int id);
    Task CreateNewbornAsync(CreateNewbornDTO newborn);
    Task UpdateNewbornAsync(int id, UpdateNewbornDTO newborn);
    Task DeleteNewbornAsync(int id);
    Task<IEnumerable<Parent>> GetNewbornParents(int id);
    Task<IEnumerable<MedicalData>> GetNewbornMedicalData(int newbornId);
    Task<IEnumerable<MedicalData>> GetNewbornMedicalDataByTime(int newbornId, DateTime time);
}