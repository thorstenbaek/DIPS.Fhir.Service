using JUST;
using System.IO;
using System.Text.Json;

namespace DIPS.Fhir.Service.Transformers
{
    public class ObservationTransformer : IObservationTransformer
    {
        private JsonTransformer JsonTransformer;
        private string Transform1;
        private string Transform2;
        private string Transform3;

        public ObservationTransformer()
        {
            JUSTContext context = new JUSTContext
            {
                EvaluationMode = EvaluationMode.Strict,
                DefaultDecimalPlaces = 4
            };

            JsonTransformer = new JsonTransformer(context);

            using (var stream = GetType().Assembly.GetManifestResourceStream("DIPS.Fhir.Service.Transformers.observation-transformer1.json"))
            {
                var streamReader = new StreamReader(stream);
                Transform1 = streamReader.ReadToEnd();
            }

            using (var stream = GetType().Assembly.GetManifestResourceStream("DIPS.Fhir.Service.Transformers.observation-transformer2.json"))
            {
                var streamReader = new StreamReader(stream);
                Transform2 = streamReader.ReadToEnd();
            }

            using (var stream = GetType().Assembly.GetManifestResourceStream("DIPS.Fhir.Service.Transformers.observation-transformer3.json"))
            {
                var streamReader = new StreamReader(stream);
                Transform3 = streamReader.ReadToEnd();
            }
        }

        public JsonElement TransformToFhir(JsonElement fhirbase)
        {
            var firstPhase = JsonTransformer.Transform(Transform1, fhirbase.ToString());
            var secondPhase = JsonTransformer.Transform(Transform2, firstPhase);
            var transformed = JsonTransformer.Transform(Transform3, secondPhase);
            
            var doc = JsonDocument.Parse(transformed);
            return doc.RootElement;
        }
    }
}