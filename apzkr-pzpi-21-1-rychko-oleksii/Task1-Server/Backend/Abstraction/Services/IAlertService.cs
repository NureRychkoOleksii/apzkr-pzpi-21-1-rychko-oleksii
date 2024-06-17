using Backend.Core.DTOs.Alert;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface IAlertService
{
    Task<IEnumerable<Alert>> GetAlertsAsync();
    Task<IEnumerable<Alert>> GetAlertsByUserAsync(int userId);
    Task<Alert> GetAlertAsync(int id);
    Task CreateAlertAsync(CreateAlertDTO alertDTO);
    Task UpdateAlertAsync(int id, UpdateAlertDTO alertDTO);
    Task DeleteAlertAsync(int id);
}