using System;
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

        [Route("BlockedApps/{computerId}")]
        [HttpPost]
        public IHttpActionResult UpdateBlockedApps(int computerId, List<BlockedAppResource> apps)
        {
            var modelApps = apps.Select(a => a.ToModel()).ToList();
            foreach (var app in modelApps)
            {
                var webAccount = _db.WebAccounts.FirstOrDefault(wa => wa.Email == User.Identity.Name);
                var computer = _db.Computers.FirstOrDefault(c => c.ComputerId == computerId);
                var user = _db.ComputerUsers.FirstOrDefault(u => u.ComputerUserId == app.ComputerUserId);
                if (user == null || user.ComputerId != computerId) continue;
                if (computer == null || webAccount == null || computer.WebAccountId != webAccount.WebAccountId) continue;


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
            try
            {
                var returnApps = modelApps.Select(u => u.ToResource(u.ComputerUser.ComputerUserId)).ToList();
                return Ok(returnApps);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("BlockedApps/{computerId}")]
        [HttpGet]
        public IHttpActionResult GetAllApps(int computerId)
        {
            var webAccount = _db.WebAccounts.FirstOrDefault(wa => wa.Email == User.Identity.Name);
            var computer = _db.Computers.FirstOrDefault(c => c.ComputerId == computerId);
            if (computer == null || webAccount == null || computer.WebAccountId != webAccount.WebAccountId) return NotFound();
            var returnApps = _db.BlockedApps.Where(a => a.ComputerUser.ComputerId == computerId).ToList().Select(a => a.ToResource(a.ComputerUserId)).ToList();
            return Ok(returnApps);
        }
    }
}