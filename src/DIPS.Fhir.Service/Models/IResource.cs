using System;
using System.Text.Json;

namespace DIPS.Fhir.Service.Models
{
    public interface IResource
    {
        string Id { get; }
        string Url { get; }
        public JsonElement Resource { get; }
    }
}
