using DIPS.Fhir.Service.Entities;
using System.Text.Json;

namespace DIPS.Fhir.Service.Models
{
    public class Practitioner : IResource
    {
        private readonly PractitionerEntity m_entity;
        private readonly string m_url;

        public string Id => m_entity.Id;
        public string Url => $"{m_url}{m_entity.Id}";
        public JsonElement Resource => m_entity.Resource;

        public Practitioner(PractitionerEntity entity, string url)
        {
            m_entity = entity;
            m_url = url;
        }
    }
}
