using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.Alert;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AlertService : IAlertService
{
    private readonly StarOfLifeContext _context;

    public AlertService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Alert>> GetAlertsAsync()
    {
        return await _context.Alerts.ToListAsync();
    }
    
    public async Task<IEnumerable<Alert>> GetAlertsByUserAsync(int userId)
    {
        return (await _context.Alerts
            .Include(a => a.Sensor)
            .ToListAsync()).Where(u => u.Sensor.NewbornId == userId);
    }

    public async Task<Alert> GetAlertAsync(int id)
    {
        return await _context.Alerts.FindAsync(id);
    }

    public async Task CreateAlertAsync(CreateAlertDTO alertDTO)
    {
        if (alertDTO == null)
        {
            throw new ArgumentNullException(nameof(alertDTO));
        }

        _context.Alerts.Add(new Alert
        {
            SensorId = alertDTO.SensorId,
            TimeAlerted = alertDTO.TimeAlerted,
            AlertMessage = alertDTO.AlertMessage,
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAlertAsync(int id, UpdateAlertDTO alertDTO)
    {
        if (alertDTO == null)
        {
            throw new ArgumentNullException(nameof(alertDTO));
        }

        var alertDb = await _context.FindAsync<Alert>(id);

        if (alertDb == null)
        {
            throw new ArgumentNullException(nameof(alertDb));
        }
        
        alertDb.SensorId = alertDTO.SensorId;
        alertDb.TimeAlerted = alertDTO.TimeAlerted;
        alertDb.AlertMessage = alertDTO.AlertMessage;

        _context.Alerts.Update(alertDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAlertAsync(int id)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert != null)
        {
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
        }
    }
}