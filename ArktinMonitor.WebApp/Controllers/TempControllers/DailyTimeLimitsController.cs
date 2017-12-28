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
    public class DailyTimeLimitsController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: DailyTimeLimits
        public ActionResult Index()
        {
            var dailyTimeLimits = db.DailyTimeLimits.Include(d => d.ComputerUser);
            return View(dailyTimeLimits.ToList());
        }

        // GET: DailyTimeLimits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTimeLimit dailyTimeLimit = db.DailyTimeLimits.Find(id);
            if (dailyTimeLimit == null)
            {
                return HttpNotFound();
            }
            return View(dailyTimeLimit);
        }

        // GET: DailyTimeLimits/Create
        public ActionResult Create()
        {
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name");
            return View();
        }

        // POST: DailyTimeLimits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DailyTimeLimitId,ComputerUserId,TimeAmount,Active")] DailyTimeLimit dailyTimeLimit)
        {
            if (ModelState.IsValid)
            {
                db.DailyTimeLimits.Add(dailyTimeLimit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", dailyTimeLimit.ComputerUserId);
            return View(dailyTimeLimit);
        }

        // GET: DailyTimeLimits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTimeLimit dailyTimeLimit = db.DailyTimeLimits.Find(id);
            if (dailyTimeLimit == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", dailyTimeLimit.ComputerUserId);
            return View(dailyTimeLimit);
        }

        // POST: DailyTimeLimits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DailyTimeLimitId,ComputerUserId,TimeAmount,Active")] DailyTimeLimit dailyTimeLimit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dailyTimeLimit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", dailyTimeLimit.ComputerUserId);
            return View(dailyTimeLimit);
        }

        // GET: DailyTimeLimits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTimeLimit dailyTimeLimit = db.DailyTimeLimits.Find(id);
            if (dailyTimeLimit == null)
            {
                return HttpNotFound();
            }
            return View(dailyTimeLimit);
        }

        // POST: DailyTimeLimits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DailyTimeLimit dailyTimeLimit = db.DailyTimeLimits.Find(id);
            db.DailyTimeLimits.Remove(dailyTimeLimit);
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
