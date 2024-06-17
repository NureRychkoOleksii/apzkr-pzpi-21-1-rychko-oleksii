using Backend.Core.Enums;

namespace Backend.Core.Entities;

public class Sensor : BaseEntity
{
    public int NewbornId { get; set; }
    public int SensorSettingsId { get; set; }
    public SensorType SensorType { get; set; }
    
    public Newborn Newborn { get; set; }
    public SensorSettings SensorSettings { get; set; }
}