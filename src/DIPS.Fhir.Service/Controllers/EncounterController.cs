using DIPS.Fhir.Service.Entities;
using DIPS.Fhir.Service.Models;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class EncounterController : ResourceController
    {
        public EncounterController(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {}

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get Encounters");

            using (var session = SessionFactory.OpenStatelessSession())
            {
                if (Request.Query.Any())
                {
                    var patientValues = Request.Query.Where(q => q.Key == "patient").Select(q => q.Value).FirstOrDefault();
                    if (patientValues.Any())
                    {
                        var patient = patientValues.First();
                        var sqlString = $"SELECT * FROM encounter o WHERE resource->'subject'->'id'->>0 = '{patient}'";

                        var sqlQuery = session.CreateSQLQuery(sqlString);
                        sqlQuery.AddEntity("p", typeof(EncounterEntity));

                        var patientEncounters = await sqlQuery.ListAsync<EncounterEntity>();
                        return Ok(new Bundle(patientEncounters.Select(p => new Encounter(p, UrlTemplate)).ToList(), "", patientEncounters.Count));
                    }

                    var key = Request.Query.Select(q => q.Key).First();
                    return NotFound(key);
                    
                }
                
                var allEncounters = await session.Query<EncounterEntity>().ToListAsync();
                return Ok(new Bundle(allEncounters.Select(p => new Encounter(p, UrlTemplate)).ToList(), "", allEncounters.Count));                
            }
        }

        [HttpGet("{id}")]        
        public async Task<IActionResult> GetById(string id)
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                var encounter = await session.Query<EncounterEntity>().Where(p => p.Id == id).FirstOrDefaultAsync();
                if (encounter != null)
                {
                    return Ok(encounter.Resource);
                }
            }

            return NotFound(id);
        }
    }
}
