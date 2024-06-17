using Backend.Core.DTOs.Sensor;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface ISensorService
{
    Task<IEnumerable<Sensor>> GetSensorsAsync();
    Task<Sensor> GetSensorAsync(int id);
    Task CreateSensorAsync(CreateSensorDTO sensorDTO);
    Task UpdateSensorAsync(int id, UpdateSensorDTO sensorDTO);
    Task DeleteSensorAsync(int id);
}