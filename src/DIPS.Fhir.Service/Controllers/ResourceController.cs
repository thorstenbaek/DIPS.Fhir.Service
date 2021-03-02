using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace DIPS.Fhir.Service.Controllers
{
    public class ResourceController : Controller
    {
        protected ISessionFactory SessionFactory;
        protected string UrlTemplate => $"{Request.Scheme}://{Request.Host}{Request.Path}/";

        public ResourceController(ISessionFactory sessionFactory)
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
    }
}
