using Backend.Core.Enums;

namespace Backend.Core.Entities;

public class Alert : BaseEntity
{
    public int SensorId { get; set; }
    public DateTime TimeAlerted { get; set; }
    public string AlertMessage { get; set; }
    public AlertType AlertType { get; set; }
    
    public Sensor Sensor { get; set; }
}