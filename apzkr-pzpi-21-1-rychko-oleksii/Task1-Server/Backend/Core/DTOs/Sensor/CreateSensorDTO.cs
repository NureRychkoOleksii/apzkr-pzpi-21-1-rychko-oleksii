using Backend.Core.Enums;

namespace Backend.Core.DTOs.Sensor;

public class CreateSensorDTO
{
    public int NewbornId { get; set; }
    public SensorType SensorType { get; set; }
    public int SensorSettingsId { get; set; }
}