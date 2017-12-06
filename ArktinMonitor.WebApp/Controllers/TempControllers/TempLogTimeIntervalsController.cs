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
    public class TempLogTimeIntervalsController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempLogTimeIntervals
        public ActionResult Index()
        {
            var logTimeIntervals = db.LogTimeIntervals.Include(l => l.Computer);
            return View(logTimeIntervals.ToList());
        }

        // GET: TempLogTimeIntervals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogTimeInterval logTimeInterval = db.LogTimeIntervals.Find(id);
            if (logTimeInterval == null)
            {
                return HttpNotFound();
            }
            return View(logTimeInterval);
        }

        // GET: TempLogTimeIntervals/Create
        public ActionResult Create()
        {
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name");
            return View();
        }

        // POST: TempLogTimeIntervals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LogTimeIntervalId,ComputerUser,ComputerId,StartTime,Duration,State")] LogTimeInterval logTimeInterval)
        {
            if (ModelState.IsValid)
            {
                db.LogTimeIntervals.Add(logTimeInterval);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", logTimeInterval.ComputerId);
            return View(logTimeInterval);
        }

        // GET: TempLogTimeIntervals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogTimeInterval logTimeInterval = db.LogTimeIntervals.Find(id);
            if (logTimeInterval == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", logTimeInterval.ComputerId);
            return View(logTimeInterval);
        }

        // POST: TempLogTimeIntervals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LogTimeIntervalId,ComputerUser,ComputerId,StartTime,Duration,State")] LogTimeInterval logTimeInterval)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logTimeInterval).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComputerId = new SelectList(db.Computers, "ComputerId", "Name", logTimeInterval.ComputerId);
            return View(logTimeInterval);
        }

        // GET: TempLogTimeIntervals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogTimeInterval logTimeInterval = db.LogTimeIntervals.Find(id);
            if (logTimeInterval == null)
            {
                return HttpNotFound();
            }
            return View(logTimeInterval);
        }

        // POST: TempLogTimeIntervals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogTimeInterval logTimeInterval = db.LogTimeIntervals.Find(id);
            db.LogTimeIntervals.Remove(logTimeInterval);
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
