using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StarOfLifeIoT.Types;

namespace StarOfLifeIoT
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var jsonString = File.ReadAllText("settings.json");
            
            var settings = JsonSerializer.Deserialize<Settings>(jsonString);
            
            var sensor = new Sensor(settings.Url, settings.LoginEndpoint, settings.SensorSettingsEndpoint, settings.MedicalDataEndpoint);

            Console.WriteLine("Enter username: ");
            var username = Console.ReadLine();
            
            Console.WriteLine("Enter password: ");
            var password = GetHiddenConsoleInput();

            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };
            
            var ventricularRepolarization = 110;
            
            await sensor.Login(loginDto);
            
            while (true)
            {
                var randNum = 5 - new Random().Next(0, 10);
                ventricularRepolarization += randNum;

                var medicalDataDto = new MedicalDataDto
                {
                    SensorData = ventricularRepolarization,
                    SensorId = settings.SensorId,
                    TimeSaved = DateTime.UtcNow
                };

                var config = await sensor.GetSensorSettings(settings.SensorSettingsId);

                await sensor.SaveData(medicalDataDto, settings.SensorSettingsId);

                await Task.Delay(config?.SamplingFrequency ?? 1000);
            }
        }
        
        private static string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }
    }
}