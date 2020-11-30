using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using DIPS.Fhir.Service.Entities;
using DIPS.Fhir.Service.Models;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Net.Mime;
using System.Text.Json;

namespace DIPS.Fhir.Service.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class PatientController : Controller
    {
        private ISessionFactory SessionFactory;

        public PatientController(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SessionFactory = null;
            }
        }

        private string CreateSQLFromRequestQuery(IEnumerable<KeyValuePair<string, StringValues>> givenQueries)
        {
            var query = givenQueries.First();

            var queryField = query.Key;
            string queryInstruction = null;
            if (queryField.Contains(":"))
            {
                var split = queryField.Split(':');
                queryField = split[0];
                queryInstruction = split[1];
            }

            string sqlString = null;

            if (queryInstruction == "exact")
            {
                sqlString = $"SELECT * FROM patient p WHERE resource->'name'->0->'{queryField}'->>0 ILIKE '{query.Value}'";
            }
            else if (queryInstruction == "contains")
            {
                sqlString = $"SELECT * FROM patient p WHERE resource->'name'->0->'{queryField}'->>0 ILIKE '%{query.Value}%'";
            }
            else
            {
                sqlString = $"SELECT * FROM patient p WHERE resource->'name'->0->'{queryField}'->>0 ILIKE '{query.Value}%'";
            }

            return sqlString;
        }

        private string UrlTemplate => $"{Request.Scheme}://{Request.Host}{Request.Path}/";

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get Patient");

            using (var session = SessionFactory.OpenStatelessSession())
            {
                if (Request.Query.Any())
                {
                    string SQLString = null;

                    var postfixQueries = Request.Query.Where(q => q.Key.StartsWith("given") || q.Key.StartsWith("family"));
                    if (postfixQueries.Any())
                    {
                        SQLString = CreateSQLFromRequestQuery(postfixQueries);
                    }

                    var dateQueries = Request.Query.Where(q => q.Key == "birthdate");
                    if (dateQueries.Any())
                    {
                        var dateQuery = dateQueries.First();
                        SQLString = $"SELECT * FROM patient p WHERE resource->'birthDate'->>0 LIKE '{dateQuery.Value}'";
                    }

                    if (!string.IsNullOrEmpty(SQLString))
                    {
                        var sqlQuery = session.CreateSQLQuery(SQLString);
                        sqlQuery.AddEntity("p", typeof(PatientEntity));

                        var filteredPatients = sqlQuery.List<PatientEntity>().ToList();
                        return Ok(new Bundle(filteredPatients.Select(p => new Patient(p, UrlTemplate)), "searchset", filteredPatients.Count));
                    }
                }

                var patients = await session.Query<PatientEntity>().ToListAsync();
                return Ok(new Bundle(patients.Select(p => new Patient(p, UrlTemplate)).ToList(), "", patients.Count));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                var patient = await session.Query<PatientEntity>().Where(p => p.Id == id).FirstOrDefaultAsync();
                if (patient != null)
                {
                    return Ok(patient.Resource);
                }
            }

            return NotFound(id);
        }
    }
}