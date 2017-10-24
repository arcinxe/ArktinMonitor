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
    public class BlockedApplicationsController : ApiController
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: api/BlockedApplications
        public IQueryable<BlockedApplication> GetBlockedApplications()
        {
            return db.BlockedApplications;
        }

        // GET: api/BlockedApplications/5
        [ResponseType(typeof(BlockedApplication))]
        public IHttpActionResult GetBlockedApplication(int id)
        {
            BlockedApplication blockedApplication = db.BlockedApplications.Find(id);
            if (blockedApplication == null)
            {
                return NotFound();
            }

            return Ok(blockedApplication);
        }

        // PUT: api/BlockedApplications/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlockedApplication(int id, BlockedApplication blockedApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blockedApplication.BlockedApplicationId)
            {
                return BadRequest();
            }

            db.Entry(blockedApplication).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockedApplicationExists(id))
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

        // POST: api/BlockedApplications
        [ResponseType(typeof(BlockedApplication))]
        public IHttpActionResult PostBlockedApplication(BlockedApplication blockedApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BlockedApplications.Add(blockedApplication);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = blockedApplication.BlockedApplicationId }, blockedApplication);
        }

        // DELETE: api/BlockedApplications/5
        [ResponseType(typeof(BlockedApplication))]
        public IHttpActionResult DeleteBlockedApplication(int id)
        {
            BlockedApplication blockedApplication = db.BlockedApplications.Find(id);
            if (blockedApplication == null)
            {
                return NotFound();
            }

            db.BlockedApplications.Remove(blockedApplication);
            db.SaveChanges();

            return Ok(blockedApplication);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlockedApplicationExists(int id)
        {
            return db.BlockedApplications.Count(e => e.BlockedApplicationId == id) > 0;
        }
    }
}