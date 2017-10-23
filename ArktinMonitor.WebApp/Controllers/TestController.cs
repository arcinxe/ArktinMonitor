using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

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
    }
}
