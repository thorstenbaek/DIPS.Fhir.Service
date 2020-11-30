using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service
{
    public class Configuration
    {
        public string FhirServiceUri { get; set; }
        public string SecurityServiceUri { get; set; }
        // public List<LabeledAddress> SmartApps { get; set; } // TODO implement this
    }

    public class CentralConfiguration : ICentralConfiguration
    {
        public string ConfigurationServiceUri => Environment.GetEnvironmentVariable("CONFIGURATION_SERVICE_URI");
        public string EnvironmentName => Environment.GetEnvironmentVariable("ENVIRONMENT");

        private Configuration Configuration;

        private async Task<Configuration> LoadConfiguration()
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{ConfigurationServiceUri}/Configuration/{EnvironmentName}";
                if (!url.StartsWith("http"))
                {
                    url = $"http://{url}";
                }

                using (var response = await httpClient.GetAsync(url))
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Configuration>(jsonString);
                }
            }
        }

        public async Task<string> GetSecurityServiceUrl()
        {
            if (Configuration == null)
            {
                Configuration = await LoadConfiguration();                    
            }

            return Configuration.SecurityServiceUri;
        }
    }
}
