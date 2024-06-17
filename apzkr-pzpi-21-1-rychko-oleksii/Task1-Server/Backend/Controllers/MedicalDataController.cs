using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.Alert;
using Backend.Core.DTOs.MedicalData;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicalDataController : ControllerBase
{
    private readonly IMedicalDataService _medicalDataService;
    private readonly IAlertService _alertService;
    private readonly ISensorService _sensorService;

    public MedicalDataController(IMedicalDataService medicalDataService, IAlertService alertService, ISensorService sensorService)
    {
        _medicalDataService = medicalDataService;
        _alertService = alertService;
        _sensorService = sensorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalData>>> GetMedicalData()
    {
        var medicalData = await _medicalDataService.GetMedicalDataAsync();
        return Ok(medicalData);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalData>> GetMedicalData(int id)
    {
        var medicalData = await _medicalDataService.GetMedicalDataAsync(id);
        if (medicalData == null)
        {
            return NotFound();
        }
        return Ok(medicalData);
    }

    [HttpPost]
    [SensorRoleInterceptor]
    public async Task<ActionResult<MedicalData>> CreateMedicalData([FromBody] CreateMedicalDataDTO medicalDataDTO)
    {
        try
        {
            await _medicalDataService.CreateMedicalDataAsync(medicalDataDTO);
            var sensor = await _sensorService.GetSensorAsync(medicalDataDTO.SensorId);

            if (sensor.SensorSettings.IsActive)
            {
                if (medicalDataDTO.SensorData > sensor.SensorSettings.HighCriticalThreshold)
                {
                    await _alertService.CreateAlertAsync(new CreateAlertDTO
                    {
                        SensorId = medicalDataDTO.SensorId,
                        AlertMessage = "High critical value!",
                        TimeAlerted = DateTime.Now
                    });
                }
                else if (medicalDataDTO.SensorData > sensor.SensorSettings.HighEdgeThreshold)
                {
                    await _alertService.CreateAlertAsync(new CreateAlertDTO
                    {
                        SensorId = medicalDataDTO.SensorId,
                        AlertMessage = "Something's not good, check out analyses. Sensor got high value!",
                        TimeAlerted = DateTime.Now
                    });
                }
                else if (medicalDataDTO.SensorData < sensor.SensorSettings.LowCriticalThreshold)
                {
                    await _alertService.CreateAlertAsync(new CreateAlertDTO
                    {
                        SensorId = medicalDataDTO.SensorId,
                        AlertMessage = "Low critical value!",
                        TimeAlerted = DateTime.Now
                    });
                }
                else if (medicalDataDTO.SensorData < sensor.SensorSettings.LowEdgeThreshold)
                {
                    await _alertService.CreateAlertAsync(new CreateAlertDTO
                    {
                        SensorId = medicalDataDTO.SensorId,
                        AlertMessage = "Something's not good, check out analyses. Sensor got low value!",
                        TimeAlerted = DateTime.Now
                    });
                }
            }
            
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
    [SensorRoleInterceptor]
    public async Task<IActionResult> UpdateMedicalData(int id, [FromBody] UpdateMedicalDataDTO medicalDataDTO)
    {
        try
        {
            await _medicalDataService.UpdateMedicalDataAsync(id, medicalDataDTO);
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
    public async Task<IActionResult> DeleteMedicalData(int id)
    {
        try
        {
            await _medicalDataService.DeleteMedicalDataAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}