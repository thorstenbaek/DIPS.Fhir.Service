using DIPS.Fhir.Service.Entities;
using DIPS.Fhir.Service.Models;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentReferenceController : ResourceController
    {
        public DocumentReferenceController(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {}

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] string patient = "")
        {
            Console.WriteLine("Get DocumentReference");

            using (var session = SessionFactory.OpenSession())
            {
                var sqlString = $"SELECT * FROM documentreference o WHERE resource->'subject'->'id'->>0 = '{patient}'";

                var sqlQuery = session.CreateSQLQuery(sqlString);
                sqlQuery.AddEntity("p", typeof(DocumentReferenceEntity));

                var documentReferences = await sqlQuery.ListAsync<DocumentReferenceEntity>();
                return Ok(new Bundle(documentReferences.Select(p => new DocumentReference(p, UrlTemplate)).ToList(), "", documentReferences.Count));
            }
        }
    }
}
