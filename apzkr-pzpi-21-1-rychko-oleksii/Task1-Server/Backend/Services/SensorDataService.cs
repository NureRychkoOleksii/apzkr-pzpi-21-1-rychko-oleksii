using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SensorDataService : ISensorDataService
{
    private readonly StarOfLifeContext _context;

    public SensorDataService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<SensorType, double>> GetAverageSensorDataAsync()
    {
        var medicalDatas = await _context.MedicalDatas
            .Include(md => md.Sensor)
            .ToListAsync();
        
        var averageData = new Dictionary<SensorType, double>();
        var count = new Dictionary<SensorType, int>();

        foreach (var medicalData in medicalDatas)
        {
            var sensorType = medicalData.Sensor.SensorType;
            
            if (!averageData.ContainsKey(sensorType))
            {
                averageData[sensorType] = 0;
                count[sensorType] = 0;
            }

            averageData[sensorType] += medicalData.SensorData;
            count[sensorType]++;
        }

        foreach (var sensorType in averageData.Keys.ToList())
        {
            if (count.TryGetValue(sensorType, out var sensorCount) && sensorCount > 0)
            {
                averageData[sensorType] /= sensorCount;
            }
        }

        return averageData;
    }
}