using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarOfLifeIoT.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StarOfLifeIoT
{
    public class Sensor
    {
        private readonly HttpClient _client;
        private readonly string _loginEndpoint;
        private readonly string _sensorSettingsEndpoint;
        private readonly string _medicalDataEndpoint;

        public Sensor(string url, string loginEndpoint, string sensorSettingsEndpoint, string medicalDataEndpoint)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            _loginEndpoint = loginEndpoint;
            _sensorSettingsEndpoint = sensorSettingsEndpoint;
            _medicalDataEndpoint = medicalDataEndpoint;
        }

        public async Task Login(LoginDto dto)
        {
            string jsonBody = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var loginResponse = await _client.PostAsync(_loginEndpoint, content);
            
            loginResponse.EnsureSuccessStatusCode();
            
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseBody);
        }

        public async Task SaveData(MedicalDataDto dto, int sensorSettingsId)
        {
            string jsonBody = JsonConvert.SerializeObject(dto);

            var config = await GetSensorSettings(sensorSettingsId);

            if (config != null && config.IsActive)
            {
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_medicalDataEndpoint, content);
                
                response.EnsureSuccessStatusCode();
                
                Console.WriteLine("Saved data successfully"); 
                Console.WriteLine($"Sensor: {dto.SensorId}, data: {dto.SensorData}, sensor low critical threshold: {config.LowCriticalThreshold}, sensor high critical threshold: {config.HighCriticalThreshold}, sensor high edge threshold: {config.HighEdgeThreshold}, sensor low edge threshold: {config.LowEdgeThreshold}");
            }
        }

        public async Task<SensorSettings> GetSensorSettings(int sensorSettingsId)
        {
            var configResponse = await _client.GetAsync(_sensorSettingsEndpoint + sensorSettingsId);
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            configResponse.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<SensorSettings>(await configResponse.Content.ReadAsStringAsync(), options);
        }
    }
}