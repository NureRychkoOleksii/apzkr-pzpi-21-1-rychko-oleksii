using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.Sensor;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SensorController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensorController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
    {
        var sensors = await _sensorService.GetSensorsAsync();
        return Ok(sensors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sensor>> GetSensor(int id)
    {
        var sensor = await _sensorService.GetSensorAsync(id);
        if (sensor == null)
        {
            return NotFound();
        }
        return Ok(sensor);
    }

    [HttpPost]
    [AdminRoleInterceptor]
    public async Task<ActionResult<Sensor>> CreateSensor([FromBody] CreateSensorDTO sensorDTO)
    {
        try
        {
            await _sensorService.CreateSensorAsync(sensorDTO);
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
    public async Task<IActionResult> UpdateSensor(int id, [FromBody] UpdateSensorDTO sensorDTO)
    {
        try
        {
            await _sensorService.UpdateSensorAsync(id, sensorDTO);
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
    public async Task<IActionResult> DeleteSensor(int id)
    {
        try
        {
            await _sensorService.DeleteSensorAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}