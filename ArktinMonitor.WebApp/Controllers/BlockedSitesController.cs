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
    public class BlockedSitesController : ApiController
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: api/BlockedSites
        public IQueryable<BlockedSite> GetBlicBlockedSites()
        {
            return db.BlicBlockedSites;
        }

        // GET: api/BlockedSites/5
        [ResponseType(typeof(BlockedSite))]
        public IHttpActionResult GetBlockedSite(int id)
        {
            BlockedSite blockedSite = db.BlicBlockedSites.Find(id);
            if (blockedSite == null)
            {
                return NotFound();
            }

            return Ok(blockedSite);
        }

        // PUT: api/BlockedSites/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlockedSite(int id, BlockedSite blockedSite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blockedSite.BlockedSiteId)
            {
                return BadRequest();
            }

            db.Entry(blockedSite).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockedSiteExists(id))
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

        // POST: api/BlockedSites
        [ResponseType(typeof(BlockedSite))]
        public IHttpActionResult PostBlockedSite(BlockedSite blockedSite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BlicBlockedSites.Add(blockedSite);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = blockedSite.BlockedSiteId }, blockedSite);
        }

        // DELETE: api/BlockedSites/5
        [ResponseType(typeof(BlockedSite))]
        public IHttpActionResult DeleteBlockedSite(int id)
        {
            BlockedSite blockedSite = db.BlicBlockedSites.Find(id);
            if (blockedSite == null)
            {
                return NotFound();
            }

            db.BlicBlockedSites.Remove(blockedSite);
            db.SaveChanges();

            return Ok(blockedSite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlockedSiteExists(int id)
        {
            return db.BlicBlockedSites.Count(e => e.BlockedSiteId == id) > 0;
        }
    }
}