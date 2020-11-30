using DIPS.Fhir.Service.Transformers;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using Xunit;

namespace TestDIPS.Fhir.Service.Transformers
{
    public class ObservationTransformerTests
    {
        private ObservationTransformer ObservationTransformer;

        public ObservationTransformerTests()
        {
            ObservationTransformer = new ObservationTransformer();
        }

        [Fact]
        public void TransformToFhir()
        {
            var fhirbaseStream = GetType().Assembly.GetManifestResourceStream("TestDIPS.Fhir.Service.Transformers.Observation-Fhirbase.json");
            var fhirbase = new StreamReader(fhirbaseStream).ReadToEnd();

            var fhirStream = GetType().Assembly.GetManifestResourceStream("TestDIPS.Fhir.Service.Transformers.Observation-Fhir.json");
            var fhir = new StreamReader(fhirStream).ReadToEnd();

            var doc = JsonDocument.Parse(fhirbase);
            var actual = ObservationTransformer.TransformToFhir(doc.RootElement);

            var expectedJObject = JObject.Parse(fhir);
            var actualJObject = JObject.Parse(actual.ToString());

            actualJObject.Should().BeEquivalentTo(expectedJObject);
        }
    }
}

