
using DIPS.Fhir.Service.Entities;
using System;
using System.Text.Json;

namespace DIPS.Fhir.Service.Models
{
    public class Observation
    {
        private readonly ObservationEntity m_entity;

        public JsonElement Resource => m_entity.Resource;

        public Observation(ObservationEntity entity)
        {
                        
        }
    }
}
