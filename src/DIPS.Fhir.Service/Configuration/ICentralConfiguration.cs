using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Configuration
{
    public interface ICentralConfiguration
    {
        public Task<string> GetSecurityServiceUrl();
    }
}
