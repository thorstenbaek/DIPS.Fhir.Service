using System.Text.Json;

namespace DIPS.Fhir.Service.Entities
{
    public class ResourceEntity
    {
        public virtual string Id { get; set; }

        public virtual JsonElement Resource { get; set; }
    }
}
