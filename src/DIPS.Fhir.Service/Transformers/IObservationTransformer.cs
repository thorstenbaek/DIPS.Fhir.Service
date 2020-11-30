using System.Text.Json;

namespace DIPS.Fhir.Service.Transformers
{
    public interface IObservationTransformer
    {
        JsonElement TransformToFhir(JsonElement fhirbase);
    }
}
