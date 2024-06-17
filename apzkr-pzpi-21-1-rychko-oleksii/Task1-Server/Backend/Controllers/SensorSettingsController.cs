using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.SensorSettings;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorSettingsController : ControllerBase
{
    private readonly ISensorSettingsService _sensorSettingsService;

    public SensorSettingsController(ISensorSettingsService sensorSettingsService)
    {
        _sensorSettingsService = sensorSettingsService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SensorSettings>>> GetSensorSettings()
    {
        var sensorSettings = await _sensorSettingsService.GetSensorSettingsAsync();
        return Ok(sensorSettings);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<SensorSettings>> GetSensorSettings(int id)
    {
        var sensorSettings = await _sensorSettingsService.GetSensorSettingsAsync(id);
        if (sensorSettings == null)
        {
            return NotFound();
        }
        return Ok(sensorSettings);
    }

    [HttpPost]
    [AdminRoleInterceptor]
    public async Task<ActionResult<SensorSettings>> CreateSensorSettings([FromBody] CreateSensorSettingsDTO sensorSettingsDTO)
    {
        try
        {
            await _sensorSettingsService.CreateSensorSettingsAsync(sensorSettingsDTO);
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
    public async Task<IActionResult> UpdateSensorSettings(int id, [FromBody] UpdateSensorSettingsDTO sensorSettingsDTO)
    {
        try
        {
            await _sensorSettingsService.UpdateSensorSettingsAsync(id, sensorSettingsDTO);
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
    public async Task<IActionResult> DeleteSensorSettings(int id)
    {
        try
        {
            await _sensorSettingsService.DeleteSensorSettingsAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}