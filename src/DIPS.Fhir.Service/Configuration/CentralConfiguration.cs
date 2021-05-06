using System;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Configuration
{
    public class Configuration
    {
        public string FhirServiceUri { get; set; }
        public string SecurityServiceUri { get; set; }
        // public List<LabeledAddress> SmartApps { get; set; } // TODO implement this
    }

    public class CentralConfiguration : ICentralConfiguration
    {
        private readonly IConfigurationLoader ConfigurationLoader;
        private readonly IEnvironment Environment;
        
        public CentralConfiguration(IConfigurationLoader configurationLoader, IEnvironment environment)
        {
            ConfigurationLoader = configurationLoader ?? throw new ArgumentNullException(nameof(configurationLoader));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<string> GetSecurityServiceUrl()
        {
            var configuration = await ConfigurationLoader.Load();                    

            var releaseName = Environment.GetEnvironmentVariable("RELEASE_NAME");
            var hostName = Environment.GetEnvironmentVariable("HOST_NAME");

            var url = configuration.SecurityServiceUri;
            url = url.Replace("RELEASE-NAME", releaseName);
            url = url.Replace("DOMAIN", hostName);

            return url;
        }
    }
}
