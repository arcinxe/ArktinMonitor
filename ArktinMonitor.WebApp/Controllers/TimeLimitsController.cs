using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class TimeLimitsController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("TimeLimits/{computerId}")]
        [HttpGet]
        public IHttpActionResult GetAllDailyTimeLimits(int computerId)
        {
            var webAccount = _db.WebAccounts.FirstOrDefault(wa => wa.Email == User.Identity.Name);
            var computer = _db.Computers.FirstOrDefault(c => c.ComputerId == computerId);
            if (computer == null || webAccount == null || computer.WebAccountId != webAccount.WebAccountId) return NotFound();
            var returnTimeLimits = _db.DailyTimeLimits.Where(a => a.ComputerUser.ComputerId == computerId).ToList().Select(a => a.ToResource()).ToList();
            return Ok(returnTimeLimits);
        }
    }
}