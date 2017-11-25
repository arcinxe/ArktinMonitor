using System.Web.Http;

namespace ArktinMonitor.WebApp.Controllers
{
    public class WebAccountController : ApiController
    {
        [HttpGet]
        [Route("api/CheckAccess")]
        public bool Get()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}