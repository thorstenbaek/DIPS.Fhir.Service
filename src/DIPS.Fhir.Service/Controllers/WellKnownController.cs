using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Route("/.well-known")]
    public class WellKnownController : Controller
    {
        private readonly ICentralConfiguration CentralConfiguration;
                
        public WellKnownController(ICentralConfiguration centralConfiguration)
        {
            CentralConfiguration = centralConfiguration;
        }

        [HttpGet("smart-configuration")]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get .WellKnown/Smart-Configuration");

            var securityServiceUrl = await CentralConfiguration.GetSecurityServiceUrl();

            var stream = GetType().Assembly.GetManifestResourceStream("DIPS.Fhir.Service.FhirData.wellknown.json");
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
