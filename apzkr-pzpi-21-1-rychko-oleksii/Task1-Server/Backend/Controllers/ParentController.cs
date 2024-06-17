using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs;
using Backend.Core.DTOs.Newborn;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ParentController : ControllerBase
{
    private readonly IParentService _parentService;

    public ParentController(IParentService parentService)
    {
        _parentService = parentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
    {
        var parents = await _parentService.GetParents();
        
        return Ok(parents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Parent>> GetParent(int id)
    {
        var parent = await _parentService.GetParentAsync(id);
        if (parent == null)
        {
            return NotFound();
        }
        return Ok(parent);
    }
    
    [HttpGet("/api/parent-user/{id}")]
    public async Task<ActionResult<Parent>> GetParentByUserId(int id)
    {
        var parents = await _parentService.GetParents();
        if (parents == null)
        {
            return NotFound();
        }

        var parentUser = parents.FirstOrDefault(p => p.UserId == id);

        if (parentUser == null)
        {
            return NotFound();
        }
        
        return Ok(parentUser);
    }

    [HttpPost]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<Parent>> CreateParent([FromBody] CreateParentDTO parent)
    {
        try
        {
            await _parentService.CreateParentAsync(parent);
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
    
    [HttpPost("withNewborn")]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<Parent>> CreateParentWithNewborn([FromBody] CreateParentWithNewbornDto dto)
    {
        try
        {
            await _parentService.CreateParentWithNewbornAsync(new CreateParentDTO
            {
                Name = dto.ParentName,
                ContractInfo = dto.ContractInfo,
                UserId = dto.ParentUserId
            }, new CreateNewbornDTO
            {
                Gender = dto.Gender,
                Name = dto.NewbornName,
                UserId = dto.NewbornUserId,
                DateOfBirth = dto.NewbornDateOfBirth
            });
            
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
    public async Task<IActionResult> UpdateParent(int id, [FromBody] UpdateParentDTO parent)
    {
        try
        {
            await _parentService.UpdateParentAsync(id, parent);
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
    public async Task<IActionResult> DeleteParent(int id)
    {
        try
        {
            await _parentService.DeleteParentAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("newborns/{id}")]
    public async Task<ActionResult<IEnumerable<Parent>>> GetNewborns(int id)
    {
        var parents = await _parentService.GetParentNewbornsAsync(id);
        
        return Ok(parents);
    }
}