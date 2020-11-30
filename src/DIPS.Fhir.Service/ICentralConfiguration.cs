using System.Threading.Tasks;

namespace DIPS.Fhir.Service
{
    public interface ICentralConfiguration
    {
        public Task<string> GetSecurityServiceUrl();
    }
}
