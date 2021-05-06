using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Configuration
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private class ConfigurationWrapper
        {
            [JsonPropertyName("settings")]
            public Configuration Settings { get; set; }
        }

        public string ConfigurationServiceUri => Environment.GetEnvironmentVariable("CONFIGURATION_SERVICE_URI");        

        public async Task<Configuration> Load()
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{ConfigurationServiceUri}/configuration.json";
                if (!url.StartsWith("http"))
                {
                    url = $"http://{url}";
                }

                using (var response = await httpClient.GetAsync(url))
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var configWrapper = JsonSerializer.Deserialize<ConfigurationWrapper>(jsonString);
                    return configWrapper.Settings;
                }
            }
        }
    }
}