using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using DIPS.Fhir.Service.Entities;
using NHibernate;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using DIPS.Fhir.Service.Transformers;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ObservationController : Controller
    {
        private ISessionFactory SessionFactory;
        private IObservationTransformer ObservationTransformer;


        public ObservationController(ISessionFactory sessionFactory, IObservationTransformer observationTransformer)
        {
            SessionFactory = sessionFactory;
            ObservationTransformer = observationTransformer;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SessionFactory = null;
            }
        }

        private string BuildFilter(IEnumerable<Tuple<string, string>> codes)
        {
            var stringBuilder = new StringBuilder();

            foreach(var c in codes)
            {
                if (c.Item1 == "http://loinc.org")
                {
                    var line = "resource @> '{\"code\": {\"coding\": [{\"code\": \"#code#\"}]}}' or \n".Replace("#code#", c.Item2);
                    stringBuilder.Append(line);
                }
            }
            
            stringBuilder.Append("false");
            
            return stringBuilder.ToString();
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] string patient = "", [FromQuery] int _count = 0, [FromQuery] string code = "")
        {
            Console.WriteLine("Get Observation");

            var codes = new List<Tuple<string, string>>();
            foreach(var c in code.Split(','))
            {
                var split = c.Split('|');
                codes.Add(new Tuple<string, string>(split[0], split[1]));                    
            }

            var subject = patient;

            if (subject != StringValues.Empty)
            {
                var codeFilter = BuildFilter(codes);

                using (var session = SessionFactory.OpenSession())
                {
                    var sqlString = $"SELECT * FROM observation o WHERE resource->'subject'->'id'->>0 = '{subject}' and ({codeFilter})";

                    var sqlQuery = session.CreateSQLQuery(sqlString);
                    sqlQuery.AddEntity("p", typeof(ObservationEntity));

                    var observations = await sqlQuery.ListAsync<ObservationEntity>();
                    return Ok(observations.Select(p => ObservationTransformer.TransformToFhir(p.Resource)));
                }
            }

            return NotFound();
        }
    }
}
