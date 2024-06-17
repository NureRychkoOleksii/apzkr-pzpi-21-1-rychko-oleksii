using Backend.Core.Enums;

namespace Backend.Abstraction.Services;

public interface ISensorDataService
{
    Task<Dictionary<SensorType, double>> GetAverageSensorDataAsync();
}
