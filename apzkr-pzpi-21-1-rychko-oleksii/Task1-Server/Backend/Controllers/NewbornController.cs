using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.Newborn;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NewbornController : ControllerBase
{
    private readonly INewbornService _newbornService;

    public NewbornController(INewbornService newbornService)
    {
        _newbornService = newbornService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Newborn>>> GetNewborns()
    {
        var newborns = await _newbornService.GetNewbornsAsync();
        return Ok(newborns);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Newborn>> GetNewborn(int id)
    {
        var newborn = await _newbornService.GetNewbornAsync(id);
        if (newborn == null)
        {
            return NotFound();
        }
        return Ok(newborn);
    }

    [HttpPost]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<Newborn>> CreateNewborn([FromBody] CreateNewbornDTO newborn)
    {
        try
        {
            await _newbornService.CreateNewbornAsync(newborn);
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
    [DoctorRoleInterceptor]
    public async Task<IActionResult> UpdateNewborn(int id, [FromBody] UpdateNewbornDTO newborn)
    {
        try
        {
            await _newbornService.UpdateNewbornAsync(id, newborn);
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
    [DoctorRoleInterceptor]
    public async Task<IActionResult> DeleteNewborn(int id)
    {
        try
        {
            await _newbornService.DeleteNewbornAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("data/{id}")]
    public async Task<IActionResult> GetNewbornData(int id)
    {
        if (id < 0)
        {
            return BadRequest("Invalid id");
        }

        try
        {
            var data = await _newbornService.GetNewbornMedicalData(id);

            return Ok(data.Select(d => new GetMedicalDataDTO
            {
                TimeSaved = d.TimeSaved,
                Sensor = d.Sensor.SensorType,
                Data = d.SensorData
            }));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}