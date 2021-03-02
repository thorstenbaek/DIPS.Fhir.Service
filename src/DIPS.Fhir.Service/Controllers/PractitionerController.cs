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
    public class PractitionerController : Controller
    {
        private ISessionFactory SessionFactory;

        public PractitionerController(ISessionFactory sessionFactory)
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

        private string UrlTemplate => $"{Request.Scheme}://{Request.Host}{Request.Path}/";

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Get Practitioner");

            using (var session = SessionFactory.OpenStatelessSession())
            {                
                var practitioners = await session.Query<PractitionerEntity>().ToListAsync();
                return Ok(new Bundle(practitioners.Select(p => new Practitioner(p, UrlTemplate)).ToList(), "", practitioners.Count));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using (var session = SessionFactory.OpenStatelessSession())
            {
                var practitioner = await session.Query<PractitionerEntity>().Where(p => p.Id == id).FirstOrDefaultAsync();
                if (practitioner != null)
                {
                    return Ok(practitioner.Resource);
                }
            }

            return NotFound(id);
        }
    }
}