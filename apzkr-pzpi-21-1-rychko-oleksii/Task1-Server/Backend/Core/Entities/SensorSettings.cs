namespace Backend.Core.Entities;

public class SensorSettings : BaseEntity
{
    public int HighCriticalThreshold { get; set; }
    public int LowCriticalThreshold { get; set; }
    public int HighEdgeThreshold { get; set; }
    public int LowEdgeThreshold { get; set; }
    public int SamplingFrequency { get; set; }
    public bool IsActive { get; set; }
}