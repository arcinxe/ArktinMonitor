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
    public class TempDisksController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempDisks
        public ActionResult Index()
        {
            var disks = db.Disks.Include(d => d.Computer);
            return View(disks.ToList());
        }

        // GET: TempDisks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disk disk = db.Disks.Find(id);
            if (disk == null)
            {
                return HttpNotFound();
            }
            return View(disk);
        }

        // GET: TempDisks/Create
        public ActionResult Create()
        {
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name");
            return View();
        }

        // POST: TempDisks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiskId,ComputerId,Letter,Name,TotalSpaceInGigaBytes,FreeSpaceInGigaBytes")] Disk disk)
        {
            if (ModelState.IsValid)
            {
                db.Disks.Add(disk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", disk.ComputerId);
            return View(disk);
        }

        // GET: TempDisks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disk disk = db.Disks.Find(id);
            if (disk == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", disk.ComputerId);
            return View(disk);
        }

        // POST: TempDisks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiskId,ComputerId,Letter,Name,TotalSpaceInGigaBytes,FreeSpaceInGigaBytes")] Disk disk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", disk.ComputerId);
            return View(disk);
        }

        // GET: TempDisks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disk disk = db.Disks.Find(id);
            if (disk == null)
            {
                return HttpNotFound();
            }
            return View(disk);
        }

        // POST: TempDisks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Disk disk = db.Disks.Find(id);
            db.Disks.Remove(disk);
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
