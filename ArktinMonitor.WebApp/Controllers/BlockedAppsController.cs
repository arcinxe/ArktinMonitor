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
    public class ComputerUserBlockedAppsControllersController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("BlockedApps")]
        [HttpPost]
        public IHttpActionResult UpdateBlocekdApps(List<BlockedAppResource> apps)
        {
            var modelApps = apps.Select(a => a.ToModel()).ToList();
            foreach (var app in modelApps)
            {
                var user = _db.ComputerUsers.FirstOrDefault(u => u.ComputerUserId == app.ComputerUserId);
                if (user == null) continue;
                var exist = _db.BlockedApps.Any(a => a.BlockedAppId == app.BlockedAppId);
                if (exist)
                {
                    _db.Entry(app).State = EntityState.Modified;
                }
                else
                {
                    _db.BlockedApps.Add(app);
                }
            }
            _db.SaveChanges();
            return Ok(modelApps.Select(u => u.ToResource(u.ComputerUser.ComputerUserId)).ToList());
        }
    }
}