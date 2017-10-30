using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class DiskController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("Disk")]
        [HttpPost]
        public IHttpActionResult GetAllDisks(ComputerResourceModel computer)
        {
           var disks = _db.Disks
                .Where(d => d.Computer.WebAccount.Email == User.Identity.Name && d.Computer.MacAddress == computer.MacAddress).ToList();
           var disks2 = disks.AsEnumerable().Select(d => d.ToResourceModel());
            return Ok(disks2);
        }
    }
}
