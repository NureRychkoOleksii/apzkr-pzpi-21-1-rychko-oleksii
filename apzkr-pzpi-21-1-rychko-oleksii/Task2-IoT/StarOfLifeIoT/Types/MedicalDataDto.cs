using System;

namespace StarOfLifeIoT.Types
{
    public class MedicalDataDto
    {
        public int SensorId { get; set; }
        public DateTime TimeSaved { get; set; }
        public int SensorData { get; set; }
    }
}