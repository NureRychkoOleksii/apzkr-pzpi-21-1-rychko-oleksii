using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.SensorSettings;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SensorSettingsService : ISensorSettingsService
{
    private readonly StarOfLifeContext _context;

    public SensorSettingsService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SensorSettings>> GetSensorSettingsAsync()
    {
        return await _context.SensorSettings.ToListAsync();
    }

    public async Task<SensorSettings> GetSensorSettingsAsync(int id)
    {
        return await _context.SensorSettings.FindAsync(id);
    }

    public async Task CreateSensorSettingsAsync(CreateSensorSettingsDTO sensorSettingsDTO)
    {
        if (sensorSettingsDTO == null)
        {
            throw new ArgumentNullException(nameof(sensorSettingsDTO));
        }

        _context.SensorSettings.Add(new SensorSettings
        {
            HighCriticalThreshold = sensorSettingsDTO.HighCriticalThreshold,
            LowCriticalThreshold = sensorSettingsDTO.LowCriticalThreshold,
            HighEdgeThreshold = sensorSettingsDTO.HighEdgeThreshold,
            LowEdgeThreshold = sensorSettingsDTO.LowEdgeThreshold,
            SamplingFrequency = sensorSettingsDTO.SamplingFrequency,
            IsActive = sensorSettingsDTO.IsActive
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateSensorSettingsAsync(int id, UpdateSensorSettingsDTO sensorSettingsDTO)
    {
        if (sensorSettingsDTO == null)
        {
            throw new ArgumentNullException(nameof(sensorSettingsDTO));
        }

        var sensorSettingsDb = await _context.FindAsync<SensorSettings>(id);

        if (sensorSettingsDb == null)
        {
            throw new ArgumentNullException(nameof(sensorSettingsDb));
        }

        sensorSettingsDb.HighCriticalThreshold = sensorSettingsDTO.HighCriticalThreshold;
        sensorSettingsDb.LowCriticalThreshold = sensorSettingsDTO.LowCriticalThreshold;
        sensorSettingsDb.HighEdgeThreshold = sensorSettingsDTO.HighEdgeThreshold;
        sensorSettingsDb.LowEdgeThreshold = sensorSettingsDTO.LowEdgeThreshold;
        sensorSettingsDb.SamplingFrequency = sensorSettingsDTO.SamplingFrequency;
        sensorSettingsDb.IsActive = sensorSettingsDTO.IsActive;

        _context.SensorSettings.Update(sensorSettingsDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSensorSettingsAsync(int id)
    {
        var sensorSettings = await _context.SensorSettings.FindAsync(id);
        if (sensorSettings != null)
        {
            _context.SensorSettings.Remove(sensorSettings);
            await _context.SaveChangesAsync();
        }
    }
}