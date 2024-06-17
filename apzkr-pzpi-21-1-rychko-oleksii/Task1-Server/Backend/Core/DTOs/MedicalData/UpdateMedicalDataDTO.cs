namespace Backend.Core.DTOs.MedicalData;

public class UpdateMedicalDataDTO
{
    public int SensorId { get; set; }
    public DateTime TimeSaved { get; set; }
    public int SensorData { get; set; }
}