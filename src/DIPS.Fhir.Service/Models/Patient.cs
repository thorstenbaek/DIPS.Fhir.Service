using DIPS.Fhir.Service.Entities;
using System;
using System.Text.Json;

namespace DIPS.Fhir.Service.Models
{
    public class Patient : IResource
    {
        private readonly PatientEntity m_entity;
        private readonly string m_url;

        public string Id => m_entity.Id;
        public string Url => $"{m_url}{m_entity.Id}";
        public JsonElement Resource => m_entity.Resource;

        public Patient(PatientEntity entity, string url)
        {
            m_entity = entity;
            m_url = url;
        }

    }
}
