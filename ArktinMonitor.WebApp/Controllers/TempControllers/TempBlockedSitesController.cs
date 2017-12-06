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
    public class TempBlockedSitesController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempBlockedSites
        public ActionResult Index()
        {
            var blicBlockedSites = db.BlicBlockedSites.Include(b => b.ComputerUser);
            return View(blicBlockedSites.ToList());
        }

        // GET: TempBlockedSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedSite blockedSite = db.BlicBlockedSites.Find(id);
            if (blockedSite == null)
            {
                return HttpNotFound();
            }
            return View(blockedSite);
        }

        // GET: TempBlockedSites/Create
        public ActionResult Create()
        {
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name");
            return View();
        }

        // POST: TempBlockedSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlockedSiteId,ComputerUserId,Name,UrlAddress,Active")] BlockedSite blockedSite)
        {
            if (ModelState.IsValid)
            {
                db.BlicBlockedSites.Add(blockedSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", blockedSite.ComputerUserId);
            return View(blockedSite);
        }

        // GET: TempBlockedSites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedSite blockedSite = db.BlicBlockedSites.Find(id);
            if (blockedSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", blockedSite.ComputerUserId);
            return View(blockedSite);
        }

        // POST: TempBlockedSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlockedSiteId,ComputerUserId,Name,UrlAddress,Active")] BlockedSite blockedSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blockedSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name", blockedSite.ComputerUserId);
            return View(blockedSite);
        }

        // GET: TempBlockedSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedSite blockedSite = db.BlicBlockedSites.Find(id);
            if (blockedSite == null)
            {
                return HttpNotFound();
            }
            return View(blockedSite);
        }

        // POST: TempBlockedSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlockedSite blockedSite = db.BlicBlockedSites.Find(id);
            db.BlicBlockedSites.Remove(blockedSite);
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
