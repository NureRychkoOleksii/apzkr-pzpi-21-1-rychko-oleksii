using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs.Analysis;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AnalysisService : IAnalysisService
{
    private readonly StarOfLifeContext _context;

    public AnalysisService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Analysis>> GetAnalysesAsync()
    {
        return await _context.Analyses.ToListAsync();
    }

    public async Task<Analysis> GetAnalysisAsync(int id)
    {
        return await _context.Analyses.FindAsync(id);
    }

    public async Task CreateAnalysisAsync(CreateAnalysisDTO analysisDTO)
    {
        if (analysisDTO == null)
        {
            throw new ArgumentNullException(nameof(analysisDTO));
        }

        _context.Analyses.Add(new Analysis
        {
            NewbornId = analysisDTO.NewbornId,
            Time = analysisDTO.Time,
            DoctorResult = analysisDTO.DoctorResult,
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAnalysisAsync(int id, UpdateAnalysisDTO analysisDTO)
    {
        if (analysisDTO == null)
        {
            throw new ArgumentNullException(nameof(analysisDTO));
        }

        var analysisDb = await _context.FindAsync<Analysis>(id);

        if (analysisDb == null)
        {
            throw new ArgumentNullException(nameof(analysisDb));
        }

        analysisDb.NewbornId = analysisDTO.NewbornId;
        analysisDb.Time = analysisDTO.Time;
        analysisDb.DoctorResult = analysisDTO.DoctorResult;

        _context.Analyses.Update(analysisDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAnalysisAsync(int id)
    {
        var analysis = await _context.Analyses.FindAsync(id);
        if (analysis != null)
        {
            _context.Analyses.Remove(analysis);
            await _context.SaveChangesAsync();
        }
    }
}