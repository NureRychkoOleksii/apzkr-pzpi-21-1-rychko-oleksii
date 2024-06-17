using Backend.Core.DTOs.Analysis;
using Backend.Core.Entities;

namespace Backend.Abstraction.Services;

public interface IAnalysisService
{
    Task<IEnumerable<Analysis>> GetAnalysesAsync();
    Task<Analysis> GetAnalysisAsync(int id);
    Task CreateAnalysisAsync(CreateAnalysisDTO analysisDTO);
    Task UpdateAnalysisAsync(int id, UpdateAnalysisDTO analysisDTO);
    Task DeleteAnalysisAsync(int id);
}