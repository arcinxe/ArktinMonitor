using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;
using System.Web.Http;
using ArktinMonitor.Data.ExtensionMethods;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class ComputerUsersController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("ComputerUsers")]
        [HttpPost]
        public IHttpActionResult UpdateComputerUser(List<ComputerUserResource> users)
        {
            var modelUsers = users.Select(u => u.ToModel()).ToList();
            foreach (var user in modelUsers)
            {
                var computer = _db.Computers.FirstOrDefault(c => c.WebAccount.Email == User.Identity.Name && c.ComputerId == user.ComputerId);
                if (user == null || computer == null) continue;
                var exist = _db.ComputerUsers.Any(u => u.ComputerUserId == user.ComputerUserId);
                if (exist)
                {
                    _db.Entry(user).State = EntityState.Modified;
                }
                else
                {
                    _db.ComputerUsers.Add(user);
                }
            }
            _db.SaveChanges();
            return Ok(modelUsers.Select(u => u.ToResource()));
        }
    }
}