using Backend.Core.Enums;

namespace Backend.Core.DTOs.Newborn;

public class GetMedicalDataDTO
{
    public SensorType Sensor { get; set; }
    public DateTime TimeSaved { get; set; }
    public int Data { get; set; }
}