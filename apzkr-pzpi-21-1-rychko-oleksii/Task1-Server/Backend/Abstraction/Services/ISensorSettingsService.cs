using Backend.Core.DTOs.SensorSettings;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface ISensorSettingsService
{
    Task<IEnumerable<SensorSettings>> GetSensorSettingsAsync();
    Task<SensorSettings> GetSensorSettingsAsync(int id);
    Task CreateSensorSettingsAsync(CreateSensorSettingsDTO sensorSettingsDTO);
    Task UpdateSensorSettingsAsync(int id, UpdateSensorSettingsDTO sensorSettingsDTO);
    Task DeleteSensorSettingsAsync(int id);
}