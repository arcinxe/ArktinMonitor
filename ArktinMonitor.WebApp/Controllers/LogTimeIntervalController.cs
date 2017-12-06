using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class LogTimeIntervalController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("LogTimeIntervals")]
        [HttpPost]
        public IHttpActionResult UpdateLogTimeIntervals(List<LogTimeIntervalResource> intervals)
        {
            var intervalsModel = intervals.Select(i => i.ToModel()).ToList();
            foreach (var interval in intervalsModel)
            {
                var computer = _db.Computers
                    .FirstOrDefault(c => c.WebAccount.Email == User.Identity.Name && c.ComputerId == interval.ComputerId);
                if (computer == null || interval == null) continue;

                var intervlaExist = _db.LogTimeIntervals.Any(i => i.LogTimeIntervalId == interval.LogTimeIntervalId);
                if (intervlaExist)
                {
                    _db.Entry(interval).State = EntityState.Modified;
                }
                else
                {
                    _db.LogTimeIntervals.Add(interval);
                }
            }
            _db.SaveChanges();
            return Ok(intervalsModel.Select(i => i.ToResource()).ToList());
        }

        //[Route("Disk")]
        //[HttpPost]
        //public IHttpActionResult GetAllDisks(ComputerResourceModel computer)
        //{
        //   var disks = _db.Disks
        //        .Where(d => d.Computer.WebAccount.Email == User.Identity.Name && d.Computer.MacAddress == computer.MacAddress).ToList();
        //   var disks2 = disks.AsEnumerable().Select(d => d.ToResourceModel());
        //    return Ok(disks2);
        //}
    }
}