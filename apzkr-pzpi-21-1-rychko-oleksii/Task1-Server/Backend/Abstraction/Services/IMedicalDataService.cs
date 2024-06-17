using Backend.Core.DTOs.MedicalData;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface IMedicalDataService
{
    Task<IEnumerable<MedicalData>> GetMedicalDataAsync();
    Task<MedicalData> GetMedicalDataAsync(int id);
    Task CreateMedicalDataAsync(CreateMedicalDataDTO medicalDataDTO);
    Task UpdateMedicalDataAsync(int id, UpdateMedicalDataDTO medicalDataDTO);
    Task DeleteMedicalDataAsync(int id);
}