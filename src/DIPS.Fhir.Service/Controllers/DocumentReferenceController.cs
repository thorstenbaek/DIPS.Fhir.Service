using DIPS.Fhir.Service.Entities;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using System;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentReferenceController : Controller
    {
        private ISessionFactory SessionFactory;

        public DocumentReferenceController(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;                
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] string patient = "")
        {
            Console.WriteLine("Get DocumentReference");

            using (var session = SessionFactory.OpenSession())
            {
                var sqlString = $"SELECT * FROM documentreference o WHERE resource->'subject'->'id'->>0 = '{patient}'";

                var sqlQuery = session.CreateSQLQuery(sqlString);
                sqlQuery.AddEntity("p", typeof(DocumentReferenceEntity));

                var observations = await sqlQuery.ListAsync<DocumentReferenceEntity>();
                return Ok(observations);
            }
        }
    }
}
