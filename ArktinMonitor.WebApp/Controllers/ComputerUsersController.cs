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
    public class ComputerUsersController : ApiController
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: api/ComputerUsers
        public IQueryable<ComputerUser> GetComputerUsers()
        {
            return db.ComputerUsers;
        }

        // GET: api/ComputerUsers/5
        [ResponseType(typeof(ComputerUser))]
        public IHttpActionResult GetComputerUser(int id)
        {
            ComputerUser computerUser = db.ComputerUsers.Find(id);
            if (computerUser == null)
            {
                return NotFound();
            }

            return Ok(computerUser);
        }

        // PUT: api/ComputerUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComputerUser(int id, ComputerUser computerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != computerUser.ComputerUserId)
            {
                return BadRequest();
            }

            db.Entry(computerUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputerUserExists(id))
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

        // POST: api/ComputerUsers
        [ResponseType(typeof(ComputerUser))]
        public IHttpActionResult PostComputerUser(ComputerUser computerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ComputerUsers.Add(computerUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = computerUser.ComputerUserId }, computerUser);
        }

        // DELETE: api/ComputerUsers/5
        [ResponseType(typeof(ComputerUser))]
        public IHttpActionResult DeleteComputerUser(int id)
        {
            ComputerUser computerUser = db.ComputerUsers.Find(id);
            if (computerUser == null)
            {
                return NotFound();
            }

            db.ComputerUsers.Remove(computerUser);
            db.SaveChanges();

            return Ok(computerUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComputerUserExists(int id)
        {
            return db.ComputerUsers.Count(e => e.ComputerUserId == id) > 0;
        }
    }
}