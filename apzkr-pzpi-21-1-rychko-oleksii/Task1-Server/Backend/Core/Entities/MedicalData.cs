namespace Backend.Core.Entities;

public class MedicalData : BaseEntity
{
    public int SensorId { get; set; }
    public DateTime TimeSaved { get; set; }
    public int SensorData { get; set; }
    
    public Sensor Sensor { get; set; }
}