using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.MedicalData;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class MedicalDataService : IMedicalDataService
{
    private readonly StarOfLifeContext _context;

    public MedicalDataService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedicalData>> GetMedicalDataAsync()
    {
        return await _context.MedicalDatas.Include(m => m.Sensor).ToListAsync();
    }

    public async Task<MedicalData> GetMedicalDataAsync(int id)
    {
        return await _context.MedicalDatas.FindAsync(id);
    }

    public async Task CreateMedicalDataAsync(CreateMedicalDataDTO medicalDataDTO)
    {
        if (medicalDataDTO == null)
        {
            throw new ArgumentNullException(nameof(medicalDataDTO));
        }

        _context.MedicalDatas.Add(new MedicalData
        {
            SensorId = medicalDataDTO.SensorId,
            TimeSaved = medicalDataDTO.TimeSaved,
            SensorData = medicalDataDTO.SensorData,
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateMedicalDataAsync(int id, UpdateMedicalDataDTO medicalDataDTO)
    {
        if (medicalDataDTO == null)
        {
            throw new ArgumentNullException(nameof(medicalDataDTO));
        }

        var medicalDataDb = await _context.FindAsync<MedicalData>(id);

        if (medicalDataDb == null)
        {
            throw new ArgumentNullException(nameof(medicalDataDb));
        }
        
        medicalDataDb.SensorId = medicalDataDTO.SensorId;
        medicalDataDb.TimeSaved = medicalDataDTO.TimeSaved;
        medicalDataDb.SensorData = medicalDataDTO.SensorData;

        _context.MedicalDatas.Update(medicalDataDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteMedicalDataAsync(int id)
    {
        var medicalData = await _context.MedicalDatas.FindAsync(id);
        if (medicalData != null)
        {
            _context.MedicalDatas.Remove(medicalData);
            await _context.SaveChangesAsync();
        }
    }
}