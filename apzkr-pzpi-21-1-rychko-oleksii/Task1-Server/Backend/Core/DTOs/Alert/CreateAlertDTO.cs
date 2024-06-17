namespace Backend.Core.DTOs.Alert;

public class CreateAlertDTO
{
    public int SensorId { get; set; }
    public DateTime TimeAlerted { get; set; }
    public string AlertMessage { get; set; }
}