using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.Newborn;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class NewbornService : INewbornService
{
    private readonly StarOfLifeContext _context;
    private readonly IMedicalDataService _medicalDataService;

    public NewbornService(StarOfLifeContext context, IMedicalDataService medicalDataService)
    {
        _context = context;
        _medicalDataService = medicalDataService;
    }

    public async Task<IEnumerable<Newborn>> GetNewbornsAsync()
    {
        return await _context.Newborns.ToListAsync();
    }

    public async Task<Newborn> GetNewbornAsync(int id)
    {
        return await _context.Newborns.FindAsync(id);
    }

    public async Task CreateNewbornAsync(CreateNewbornDTO newborn)
    {
        if (newborn == null)
        {
            throw new ArgumentNullException(nameof(newborn));
        }

        _context.Newborns.Add(new Newborn
        {
            UserId = newborn.UserId,
            Name = newborn.Name,
            DateOfBirth = newborn.DateOfBirth,
            Gender = newborn.Gender,
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateNewbornAsync(int id, UpdateNewbornDTO newborn)
    {
        if (newborn == null)
        {
            throw new ArgumentNullException(nameof(newborn));
        }

        var newbornDb = await _context.Newborns.FindAsync(id);

        if (newbornDb == null)
        {
            throw new ArgumentNullException(nameof(newborn));
        }

        newbornDb.Name = newborn.Name;
        newbornDb.DateOfBirth = newborn.DateOfBirth;
        newbornDb.Gender = newborn.Gender;

        _context.Newborns.Update(newbornDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteNewbornAsync(int id)
    {
        var newborn = await _context.Newborns.FindAsync(id);
        if (newborn != null)
        {
            _context.Newborns.Remove(newborn);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Parent>> GetNewbornParents(int id)
    {
        var newborn = await _context.Newborns.Include(n => n.UserParents).FirstOrDefaultAsync(n => n.Id == id);
        
        if (newborn != null)
        {
            return await _context.Parents.Where(p => newborn.UserParents.Any(up => up.ParentId == p.Id)).ToListAsync();
        }

        return null;
    }

    public async Task<IEnumerable<MedicalData>> GetNewbornMedicalData(int newbornId)
    {
        var medicalDatas = await _medicalDataService.GetMedicalDataAsync();

        return medicalDatas.Where(m => m.Sensor.NewbornId == newbornId);
    }
    
    public async Task<IEnumerable<MedicalData>> GetNewbornMedicalDataByTime(int newbornId, DateTime time)
    {
        var medicalDatas = await _medicalDataService.GetMedicalDataAsync();

        return medicalDatas.Where(m => m.Sensor.NewbornId == newbornId && m.TimeSaved >= time);
    }
}