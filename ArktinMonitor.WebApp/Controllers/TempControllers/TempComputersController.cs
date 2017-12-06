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
    public class TempComputersController : Controller
    {
        private ArktinMonitorDataAccess db = new ArktinMonitorDataAccess();

        // GET: TempComputers
        public ActionResult Index()
        {
            var computers = db.Computers.Include(c => c.WebAccount);
            return View(computers.ToList());
        }

        // GET: TempComputers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return HttpNotFound();
            }
            return View(computer);
        }

        // GET: TempComputers/Create
        public ActionResult Create()
        {
            ViewBag.WebAccountId = new SelectList(db.WebAccounts, "WebAccountId", "Name");
            return View();
        }

        // POST: TempComputers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComputerId,Ram,WebAccountId,Name,Cpu,Gpu,OperatingSystem,MacAddress")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                db.Computers.Add(computer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WebAccountId = new SelectList(db.WebAccounts, "WebAccountId", "Name", computer.WebAccountId);
            return View(computer);
        }

        // GET: TempComputers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebAccountId = new SelectList(db.WebAccounts, "WebAccountId", "Name", computer.WebAccountId);
            return View(computer);
        }

        // POST: TempComputers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComputerId,Ram,WebAccountId,Name,Cpu,Gpu,OperatingSystem,MacAddress")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(computer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WebAccountId = new SelectList(db.WebAccounts, "WebAccountId", "Name", computer.WebAccountId);
            return View(computer);
        }

        // GET: TempComputers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return HttpNotFound();
            }
            return View(computer);
        }

        // POST: TempComputers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Computer computer = db.Computers.Find(id);
            db.Computers.Remove(computer);
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
