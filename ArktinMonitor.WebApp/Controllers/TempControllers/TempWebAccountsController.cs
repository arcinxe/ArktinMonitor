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
    public class TempWebAccountsController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempWebAccounts
        public ActionResult Index()
        {
            return View(db.WebAccounts.ToList());
        }

        // GET: TempWebAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebAccount webAccount = db.WebAccounts.Find(id);
            if (webAccount == null)
            {
                return HttpNotFound();
            }
            return View(webAccount);
        }

        // GET: TempWebAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempWebAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WebAccountId,Name,Email")] WebAccount webAccount)
        {
            if (ModelState.IsValid)
            {
                db.WebAccounts.Add(webAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(webAccount);
        }

        // GET: TempWebAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebAccount webAccount = db.WebAccounts.Find(id);
            if (webAccount == null)
            {
                return HttpNotFound();
            }
            return View(webAccount);
        }

        // POST: TempWebAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WebAccountId,Name,Email")] WebAccount webAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(webAccount);
        }

        // GET: TempWebAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebAccount webAccount = db.WebAccounts.Find(id);
            if (webAccount == null)
            {
                return HttpNotFound();
            }
            return View(webAccount);
        }

        // POST: TempWebAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WebAccount webAccount = db.WebAccounts.Find(id);
            db.WebAccounts.Remove(webAccount);
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
