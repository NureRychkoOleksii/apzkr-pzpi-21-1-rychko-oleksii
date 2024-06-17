namespace Backend.Core.DTOs.SensorSettings;

public class CreateSensorSettingsDTO
{
    public int HighCriticalThreshold { get; set; }
    public int LowCriticalThreshold { get; set; }
    public int HighEdgeThreshold { get; set; }
    public int LowEdgeThreshold { get; set; }
    public int SamplingFrequency { get; set; }
    public bool IsActive { get; set; }
}