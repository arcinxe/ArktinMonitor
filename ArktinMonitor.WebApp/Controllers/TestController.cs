using ArktinMonitor.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArktinMonitor.WebApp.Controllers
{
    [RoutePrefix("Test")]
    public class TestController : ApiController
    {
        [Route("HelloWorld")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage HelloWorld()
        {
            var userEmail = "";
            if (User.Identity.IsAuthenticated)
            {
                userEmail = User.Identity.Name;
            }
            return Request.CreateResponse(HttpStatusCode.OK, $"Hello {userEmail}");
        }

        [Route("Hello")]
        [HttpGet]
        public HttpResponseMessage Test()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello");
        }

        [Route("Computer")]
        [HttpGet]
        public IHttpActionResult Computer()
        {
            var db = new ArktinMonitorDataAccess();
            var computerName = db.Computers.FirstOrDefault().Name;
            return Ok(computerName);
        }
    }
}