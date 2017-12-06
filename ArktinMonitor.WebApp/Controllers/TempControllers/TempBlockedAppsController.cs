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
    public class TempBlockedAppsController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempBlockedApps
        public ActionResult Index()
        {
            var blockedApps = db.BlockedApps.Include(b => b.ComputerUser);
            return View(blockedApps.ToList());
        }

        // GET: TempBlockedApps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedApp blockedApp = db.BlockedApps.Find(id);
            if (blockedApp == null)
            {
                return HttpNotFound();
            }
            return View(blockedApp);
        }

        // GET: TempBlockedApps/Create
        public ActionResult Create()
        {
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name");
            return View();
        }

        // POST: TempBlockedApps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlockedAppId,ComputerUserId,Name,Path,Active")] BlockedApp blockedApp)
        {
            if (ModelState.IsValid)
            {
                db.BlockedApps.Add(blockedApp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", blockedApp.ComputerUserId);
            return View(blockedApp);
        }

        // GET: TempBlockedApps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedApp blockedApp = db.BlockedApps.Find(id);
            if (blockedApp == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", blockedApp.ComputerUserId);
            return View(blockedApp);
        }

        // POST: TempBlockedApps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlockedAppId,ComputerUserId,Name,Path,Active")] BlockedApp blockedApp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blockedApp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", blockedApp.ComputerUserId);
            return View(blockedApp);
        }

        // GET: TempBlockedApps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedApp blockedApp = db.BlockedApps.Find(id);
            if (blockedApp == null)
            {
                return HttpNotFound();
            }
            return View(blockedApp);
        }

        // POST: TempBlockedApps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlockedApp blockedApp = db.BlockedApps.Find(id);
            db.BlockedApps.Remove(blockedApp);
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
