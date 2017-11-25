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
    public class DiskController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("Disks")]
        [HttpPost]
        public IHttpActionResult UpdateDisks(List<DiskResourceModel> disks)
        {
            var disksModel = disks.Select(d => d.ToModel()).ToList();
            var returnDisks = new List<Disk>();
            foreach (var disk in disksModel)
            {
                returnDisks.Add(disk);
                var computer = _db.Computers.FirstOrDefault(c => c.ComputerId == disk.ComputerId);
                if (computer == null || disk == null) return NotFound();

                var diskExist = _db.Disks.Any(d => d.DiskId == disk.DiskId);
                if (diskExist)
                {
                    _db.Entry(disk).State = EntityState.Modified;
                }
                else
                {
                    _db.Disks.Add(disk);
                }
            }
            _db.SaveChanges();
            return Ok(returnDisks.Select(d => d.ToResourceModel()));
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