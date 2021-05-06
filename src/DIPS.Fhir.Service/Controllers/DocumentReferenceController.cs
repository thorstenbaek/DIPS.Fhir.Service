using DIPS.Fhir.Service.Entities;
using DIPS.Fhir.Service.Models;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
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
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get DocumentReference");

            using (var session = SessionFactory.OpenSession())
            {
                if (Request.Query.Any())
                {
                    var patientQuery = Request.Query.Where(q => q.Key.ToLower() == "patient").FirstOrDefault();
                    
                    var sqlString = $"SELECT * FROM documentreference o WHERE resource->'subject'->'id'->>0 = '{patientQuery.Value}'";

                    var sqlQuery = session.CreateSQLQuery(sqlString);
                    sqlQuery.AddEntity("p", typeof(DocumentReferenceEntity));

                    var documentReferences = await sqlQuery.ListAsync<DocumentReferenceEntity>();
                    return Ok(new Bundle(documentReferences.Select(p => new DocumentReference(p, UrlTemplate)).ToList(), "", documentReferences.Count));
                    
                }
                
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                var documentReference = await session.Query<DocumentReferenceEntity>().Where(p => p.Id == id).FirstOrDefaultAsync();
                if (documentReference != null)
                {
                    return Ok(documentReference.Resource);
                }
            }

            return NotFound(id);
        }
    }
}
