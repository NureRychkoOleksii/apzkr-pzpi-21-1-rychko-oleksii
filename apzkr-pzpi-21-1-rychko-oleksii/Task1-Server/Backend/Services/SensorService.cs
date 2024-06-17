using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.Sensor;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SensorService : ISensorService
{
    private readonly StarOfLifeContext _context;

    public SensorService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sensor>> GetSensorsAsync()
    {
        return await _context.Sensors.Include(s => s.SensorSettings).ToListAsync();
    }

    public async Task<Sensor> GetSensorAsync(int id)
    {
        return (await _context.Sensors
                .Include(s => s.SensorSettings)
                .ToListAsync())
            .FirstOrDefault(s => s.Id == id);
    }

    public async Task CreateSensorAsync(CreateSensorDTO sensorDTO)
    {
        if (sensorDTO == null)
        {
            throw new ArgumentNullException(nameof(sensorDTO));
        }

        _context.Sensors.Add(new Sensor
        {
            NewbornId = sensorDTO.NewbornId,
            SensorType = sensorDTO.SensorType,
            SensorSettingsId = sensorDTO.SensorSettingsId,
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateSensorAsync(int id, UpdateSensorDTO sensorDTO)
    {
        if (sensorDTO == null)
        {
            throw new ArgumentNullException(nameof(sensorDTO));
        }

        var sensorDb = await _context.FindAsync<Sensor>(id);

        if (sensorDb == null)
        {
            throw new ArgumentNullException(nameof(sensorDb));
        }

        sensorDb.NewbornId = sensorDTO.NewbornId;
        sensorDb.SensorType = sensorDTO.SensorType;
        sensorDb.SensorSettingsId = sensorDTO.SensorSettingsId;

        _context.Sensors.Update(sensorDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSensorAsync(int id)
    {
        var sensor = await _context.Sensors.FindAsync(id);
        if (sensor != null)
        {
            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();
        }
    }
}