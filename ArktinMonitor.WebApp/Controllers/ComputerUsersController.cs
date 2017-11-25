using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;
using System.Web.Http;

namespace ArktinMonitor.WebApp.Controllers
{
    public class ComputerUsersController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("ComputerUsers")]
        [HttpPost]
        public IHttpActionResult UpdateComputerUser(ComputerUser user)
        {

            _db.SaveChanges();
            return Ok();
        }
    }
}