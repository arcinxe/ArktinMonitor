using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ArktinMonitor.Models;

namespace ArktinMonitor.WebApp.Controllers
{
    public class DisksController : ApiController
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: api/Disks
        public IQueryable<Disk> GetDisks()
        {
            return db.Disks;
        }

        // GET: api/Disks/5
        [ResponseType(typeof(Disk))]
        public IHttpActionResult GetDisk(int id)
        {
            Disk disk = db.Disks.Find(id);
            if (disk == null)
            {
                return NotFound();
            }

            return Ok(disk);
        }

        // PUT: api/Disks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDisk(int id, Disk disk)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != disk.DiskId)
            {
                return BadRequest();
            }

            db.Entry(disk).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Disks
        [ResponseType(typeof(Disk))]
        public IHttpActionResult PostDisk(Disk disk)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Disks.Add(disk);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = disk.DiskId }, disk);
        }

        // DELETE: api/Disks/5
        [ResponseType(typeof(Disk))]
        public IHttpActionResult DeleteDisk(int id)
        {
            Disk disk = db.Disks.Find(id);
            if (disk == null)
            {
                return NotFound();
            }

            db.Disks.Remove(disk);
            db.SaveChanges();

            return Ok(disk);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiskExists(int id)
        {
            return db.Disks.Count(e => e.DiskId == id) > 0;
        }
    }
}