using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.IO;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetadataController : Controller
    {
        private readonly ICentralConfiguration CentralConfiguration; 
        
        public MetadataController(ICentralConfiguration centralConfiguration)
        {
            CentralConfiguration = centralConfiguration;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get Metadata");

            var securityServiceUrl = await CentralConfiguration.GetSecurityServiceUrl();

            var stream = this.GetType().Assembly.GetManifestResourceStream("DIPS.Fhir.Service.FhirData.metadata.json");
            if (stream == null)
            {
                return NotFound();
            }

            var jsonData = await new StreamReader(stream).ReadToEndAsync();
            jsonData = jsonData.Replace("${SecurityServiceUri}", securityServiceUrl);

            return Ok(JsonDocument.Parse(jsonData).RootElement);
        }
    }
}
