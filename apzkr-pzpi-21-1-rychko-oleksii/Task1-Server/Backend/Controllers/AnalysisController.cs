using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.Analysis;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalysisController : ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Analysis>>> GetAnalyses()
    {
        var analyses = await _analysisService.GetAnalysesAsync();
        return Ok(analyses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Analysis>> GetAnalysis(int id)
    {
        var analysis = await _analysisService.GetAnalysisAsync(id);
        if (analysis == null)
        {
            return NotFound();
        }
        return Ok(analysis);
    }

    [HttpPost]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<Analysis>> CreateAnalysis([FromBody] CreateAnalysisDTO analysisDTO)
    {
        try
        {
            await _analysisService.CreateAnalysisAsync(analysisDTO);
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
    public async Task<IActionResult> UpdateAnalysis(int id, [FromBody] UpdateAnalysisDTO analysisDTO)
    {
        try
        {
            await _analysisService.UpdateAnalysisAsync(id, analysisDTO);
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
    public async Task<IActionResult> DeleteAnalysis(int id)
    {
        try
        {
            await _analysisService.DeleteAnalysisAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}