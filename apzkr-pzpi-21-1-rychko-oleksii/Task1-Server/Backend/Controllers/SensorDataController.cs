using Backend.Abstraction.Services;
using Backend.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorDataController : ControllerBase
{
    private readonly ISensorDataService _sensorDataService;

    public SensorDataController(ISensorDataService sensorDataService)
    {
        _sensorDataService = sensorDataService;
    }

    [HttpGet("average")]
    public async Task<ActionResult<Dictionary<SensorType, double>>> GetAverageSensorData()
    {
        var averageSensorData = await _sensorDataService.GetAverageSensorDataAsync();
        return Ok(averageSensorData);
    }
}