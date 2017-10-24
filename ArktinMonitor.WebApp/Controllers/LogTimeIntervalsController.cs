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
    public class LogTimeIntervalsController : ApiController
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: api/LogTimeIntervals
        public IQueryable<LogTimeInterval> GetLogTimeIntervals()
        {
            return db.LogTimeIntervals;
        }

        // GET: api/LogTimeIntervals/5
        [ResponseType(typeof(LogTimeInterval))]
        public IHttpActionResult GetLogTimeInterval(string id)
        {
            LogTimeInterval logTimeInterval = db.LogTimeIntervals.Find(id);
            if (logTimeInterval == null)
            {
                return NotFound();
            }

            return Ok(logTimeInterval);
        }

        // PUT: api/LogTimeIntervals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLogTimeInterval(string id, LogTimeInterval logTimeInterval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logTimeInterval.LogTimeIntervalId)
            {
                return BadRequest();
            }

            db.Entry(logTimeInterval).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogTimeIntervalExists(id))
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

        // POST: api/LogTimeIntervals
        [ResponseType(typeof(LogTimeInterval))]
        public IHttpActionResult PostLogTimeInterval(LogTimeInterval logTimeInterval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LogTimeIntervals.Add(logTimeInterval);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LogTimeIntervalExists(logTimeInterval.LogTimeIntervalId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = logTimeInterval.LogTimeIntervalId }, logTimeInterval);
        }

        // DELETE: api/LogTimeIntervals/5
        [ResponseType(typeof(LogTimeInterval))]
        public IHttpActionResult DeleteLogTimeInterval(string id)
        {
            LogTimeInterval logTimeInterval = db.LogTimeIntervals.Find(id);
            if (logTimeInterval == null)
            {
                return NotFound();
            }

            db.LogTimeIntervals.Remove(logTimeInterval);
            db.SaveChanges();

            return Ok(logTimeInterval);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogTimeIntervalExists(string id)
        {
            return db.LogTimeIntervals.Count(e => e.LogTimeIntervalId == id) > 0;
        }
    }
}