using System;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebGrease.Css.Extensions;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class DiskController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("Disks")]
        [HttpPost]
        public IHttpActionResult UpdateDisks(List<DiskResource> disks)
        {
            var computerId = disks.FirstOrDefault()?.ComputerId;
            if (disks.Any(d => d.ComputerId != computerId)) NotFound();
            var disksModel = disks.Select(d => d.ToModel()).ToList();
            var computer = _db.Computers.AsNoTracking()
                .FirstOrDefault(c => c.WebAccount.Email == User.Identity.Name && c.ComputerId == computerId);

            if (computer == null) NotFound();
            //_db.Disks.Where(d => d.ComputerId == computerId).ForEach(d => d.Removed = true);
            //_db.SaveChanges();

            foreach (var disk in disksModel)
            {
                var diskExist = _db.Disks.Any(d => d.DiskId == disk.DiskId);

                if (!diskExist)
                {
                    var maybeExistingDiskId = _db.Disks.AsNoTracking().FirstOrDefault(d => d.ComputerId == computerId && Math.Abs(d.TotalSpaceInGigaBytes - disk.TotalSpaceInGigaBytes) < 0.01 && d.Name == disk.Name)?.DiskId;
                    if (maybeExistingDiskId > 0)
                    {
                        disk.DiskId = maybeExistingDiskId.Value;
                        diskExist = true;
                    }
                }

                if (diskExist)
                {
                    _db.Entry(disk).State = EntityState.Modified;
                }
                else
                {
                    _db.Disks.Add(disk);
                }
            }
            var disksIds = disksModel.Select(d => d.DiskId).ToArray();
            var removedDisks = _db.Disks
                .Where(d => d.ComputerId == computerId && !disksIds.Contains(d.DiskId)).ToList();
            foreach (var disk in removedDisks)
            {
                disk.Removed = true;
            }

            _db.SaveChanges();
            var returnDisks = _db.Disks.Where(d => !d.Removed && d.ComputerId == computerId).ToList();
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