using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Configuration
{
    public interface IConfigurationLoader
    {
        public Task<Configuration> Load();
    };
}