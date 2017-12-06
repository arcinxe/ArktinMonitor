using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.WebApp.Controllers.TempControllers
{
    [Authorize]
    public class TempComputerUsersController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempComputerUsers
        public ActionResult Index()
        {
            var computerUsers = db.ComputerUsers.Include(c => c.Computer);
            return View(computerUsers.ToList());
        }

        // GET: TempComputerUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComputerUser computerUser = db.ComputerUsers.Find(id);
            if (computerUser == null)
            {
                return HttpNotFound();
            }
            return View(computerUser);
        }

        // GET: TempComputerUsers/Create
        public ActionResult Create()
        {
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name");
            return View();
        }

        // POST: TempComputerUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComputerUserId,Removed,ComputerId,Name,FullName,PrivilegeLevel")] ComputerUser computerUser)
        {
            if (ModelState.IsValid)
            {
                db.ComputerUsers.Add(computerUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", computerUser.ComputerId);
            return View(computerUser);
        }

        // GET: TempComputerUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComputerUser computerUser = db.ComputerUsers.Find(id);
            if (computerUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", computerUser.ComputerId);
            return View(computerUser);
        }

        // POST: TempComputerUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComputerUserId,Removed,ComputerId,Name,FullName,PrivilegeLevel")] ComputerUser computerUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(computerUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", computerUser.ComputerId);
            return View(computerUser);
        }

        // GET: TempComputerUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComputerUser computerUser = db.ComputerUsers.Find(id);
            if (computerUser == null)
            {
                return HttpNotFound();
            }
            return View(computerUser);
        }

        // POST: TempComputerUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComputerUser computerUser = db.ComputerUsers.Find(id);
            db.ComputerUsers.Remove(computerUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
