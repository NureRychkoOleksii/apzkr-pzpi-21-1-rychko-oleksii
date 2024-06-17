using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.Alert;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts()
    {
        var alerts = await _alertService.GetAlertsAsync();
        return Ok(alerts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Alert>> GetAlert(int id)
    {
        var alert = await _alertService.GetAlertAsync(id);
        if (alert == null)
        {
            return NotFound();
        }
        return Ok(alert);
    }
    
    [HttpGet("newborn/{id}")]
    public async Task<ActionResult<Alert>> GetAlertByUser(int id)
    {
        var alerts = await _alertService.GetAlertsByUserAsync(id);
        if (alerts == null || !alerts.Any()) 
        {
            return NotFound();
        }
        return Ok(alerts);
    }

    [HttpPost]
    [SensorRoleInterceptor]
    public async Task<ActionResult<Alert>> CreateAlert([FromBody] CreateAlertDTO alertDTO)
    {
        try
        {
            await _alertService.CreateAlertAsync(alertDTO);
            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> UpdateAlert(int id, [FromBody] UpdateAlertDTO alertDTO)
    {
        try
        {
            await _alertService.UpdateAlertAsync(id, alertDTO);
            return NoContent();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> DeleteAlert(int id)
    {
        try
        {
            await _alertService.DeleteAlertAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}