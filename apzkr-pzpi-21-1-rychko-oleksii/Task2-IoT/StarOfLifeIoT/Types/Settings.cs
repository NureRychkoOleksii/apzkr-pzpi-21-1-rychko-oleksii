using System;

namespace StarOfLifeIoT.Types
{
    public class Settings
    {
        public string Url { get;set; }
        public string LoginEndpoint { get;set; }
        public string SensorSettingsEndpoint { get;set; }
        public string MedicalDataEndpoint { get;set; }
        public int SensorSettingsId { get; set; }
        public int SensorId { get; set; }
    }
}