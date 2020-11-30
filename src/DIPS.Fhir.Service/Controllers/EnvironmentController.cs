using System;
using Microsoft.AspNetCore.Mvc;

namespace my_postgres_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvironmentController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            Console.WriteLine("Get Environment");

            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment))
            {
                return Ok(environment);
            }

            return NotFound();
        }
    }
}
