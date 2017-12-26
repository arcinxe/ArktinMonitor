﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;
using ArktinMonitor.WebApp.ViewModels;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();
        [Route("Users/{computerId}")]
        public ActionResult Users(int computerId)
        {
            var computer = _db.Computers.FirstOrDefault(c => c.ComputerId == computerId /*&& c.WebAccount.Email == User.Identity.Name*/);
            var users = _db.ComputerUsers.Where(u => u.ComputerId == computerId).ToList();
            var viewModel = new ComputerUsersViewModel
            {
                ComputerName = computer?.Name,
                ComputerId = computer.ComputerId,
                Users = new List<ComputerUserViewModel>()
            };
            foreach (var user in users)
            {
                var viewModelUser = new ComputerUserViewModel
                {
                    BlockedApps = _db.BlockedApps.Where(a => a.ComputerUserId == user.ComputerUserId).ToList(),
                    BlockedSites = _db.BlockedSites.Where(s => s.ComputerUserId == user.ComputerUserId).ToList(),
                    Details = user
                };
                viewModel.Users.Add(viewModelUser);

            }
            if (computer == null) return View("Error");
            return View(viewModel);
        }

        // GET: TempBlockedSites/Create
        [Route("Users/{computerId}/AddSite")]
        public ActionResult AddSite(int computerId)
        {
            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers.Where(u => u.ComputerId == computerId), "ComputerUserId", "Name");
            //ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers, "ComputerUserId", "Name");
            return View();
        }


        // POST: TempBlockedSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/{computerId}/AddSite")]
        public ActionResult AddSite([Bind(Include = "BlockedSiteId,ComputerUserId,Name,UrlAddress,Active")] BlockedSite blockedSite)
        {
            if (ModelState.IsValid)
            {
                _db.BlockedSites.Add(blockedSite);
                _db.SaveChanges();
                var site =_db.BlockedSites.FirstOrDefault(s => s.BlockedSiteId == blockedSite.BlockedSiteId);
                if (site?.ComputerUser != null) ViewBag.ComputerId = site.ComputerUser.ComputerId;
                return RedirectToAction("Users");
            }

            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers, "ComputerUserId", "Name", blockedSite.ComputerUserId);
            return View(blockedSite);
        }

        // GET: TempBlockedSites/Edit/5
        [Route("Users/{siteId}/EditSite")]
        public ActionResult EditSite(int? siteId)
        {
            if (siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedSite blockedSite = _db.BlockedSites.Find(siteId);
            if (blockedSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = blockedSite.ComputerUser.ComputerId;
            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers.Where(u => u.ComputerId == blockedSite.ComputerUser.ComputerId), "ComputerUserId", "Name", blockedSite.ComputerUserId);
            return View(blockedSite);
        }

        // POST: TempBlockedSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/{siteId}/EditSite")]
        public ActionResult EditSite([Bind(Include = "BlockedSiteId,ComputerUserId,Name,UrlAddress,Active")] BlockedSite blockedSite)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(blockedSite).State = EntityState.Modified;
                _db.SaveChanges();
    
                return RedirectToAction("Users","Users", new {computerId = _db.ComputerUsers.FirstOrDefault(u => u.ComputerUserId == blockedSite.ComputerUserId)?.ComputerId });
            }
            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers.Where(u => u.ComputerId == blockedSite.ComputerUser.ComputerId), "ComputerUserId", "Name", blockedSite.ComputerUserId);
            return View(blockedSite);
        }


        // GET: TempBlockedSites/Delete/5
        [Route("Users/{siteId}/DeleteSite")]
        public ActionResult DeleteSite(int? siteId)
        {
            if (siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedSite blockedSite = _db.BlockedSites.Find(siteId);
            if (blockedSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = blockedSite.ComputerUser.ComputerId;
            return View(blockedSite);
        }

        // POST: TempBlockedSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Users/{siteId}/DeleteSite")]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteSiteConfirmed(int siteId)
        {
            BlockedSite blockedSite = _db.BlockedSites.Find(siteId);
            var computerId = blockedSite?.ComputerUser.ComputerId;
            _db.BlockedSites.Remove(blockedSite);
            _db.SaveChanges();
             return RedirectToAction("Users","Users", new {computerId = computerId });
        }

        // GET: TempBlockedSites/Details/5
        [Route("Users/{siteId}/SiteDetails")]
        public ActionResult SiteDetails(int? siteId)
        {
            if (siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedSite blockedSite = _db.BlockedSites.Find(siteId);
            if (blockedSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = blockedSite.ComputerUser.ComputerId;
            return View(blockedSite);
        }


        // GET: TempBlockedApps/Details/5
        [Route("Users/{appId}/AppDetails")]
        public ActionResult AppDetails(int? appId)
        {
            if (appId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedApp blockedApp = _db.BlockedApps.Find(appId);
            if (blockedApp == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = _db.BlockedApps.FirstOrDefault(a => a.BlockedAppId == appId).ComputerUser.ComputerId;

            return View(blockedApp);
        }

        // GET: TempBlockedApps/Create
        [Route("Users/{computerId}/AddApp")]
        public ActionResult AddApp(int computerId)
        {
            //ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers, "ComputerUserId", "Name");
            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers.Where(u => u.ComputerId == computerId), "ComputerUserId", "Name");
            return View();
        }

        // POST: TempBlockedApps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/{computerId}/AddApp")]
        public ActionResult AddApp([Bind(Include = "BlockedAppId,ComputerUserId,Name,Path,Active")] BlockedApp blockedApp)
        {
            if (ModelState.IsValid)
            {
                _db.BlockedApps.Add(blockedApp);
                _db.SaveChanges();
                return RedirectToAction("Users");
            }

            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers, "ComputerUserId", "Name", blockedApp.ComputerUserId);
            return View(blockedApp);
        }

        // GET: TempBlockedApps/Edit/5
        [Route("Users/{appId}/EditApp")]
        public ActionResult EditApp(int? appId)
        {
            if (appId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedApp blockedApp = _db.BlockedApps.Find(appId);
            if (blockedApp == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = blockedApp.ComputerUser.ComputerId;
            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers.Where(u => u.ComputerId == blockedApp.ComputerUser.ComputerId), "ComputerUserId", "Name", blockedApp.ComputerUserId);
            //ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers, "ComputerUserId", "Name", blockedApp.ComputerUserId);
            return View(blockedApp);
        }

        // POST: TempBlockedApps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/{appId}/EditApp")]
        public ActionResult EditApp([Bind(Include = "BlockedAppId,ComputerUserId,Name,Path,Active")] BlockedApp blockedApp)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(blockedApp).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Users", "Users", new { computerId = _db.ComputerUsers.FirstOrDefault(u => u.ComputerUserId == blockedApp.ComputerUserId)?.ComputerId });
            }
            ViewBag.ComputerUserId = new SelectList(_db.ComputerUsers.Where(u => u.ComputerId == blockedApp.ComputerUser.ComputerId), "ComputerUserId", "Name", blockedApp.ComputerUserId);
            return View(blockedApp);
        }

        // GET: TempBlockedApps/Delete/5
        [Route("Users/{appId}/DeleteApp")]
        public ActionResult DeleteApp(int? appId)
        {
            if (appId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlockedApp blockedApp = _db.BlockedApps.Find(appId);
            if (blockedApp == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComputerId = blockedApp.ComputerUser.ComputerId;

            return View(blockedApp);
        }

        // POST: TempBlockedApps/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Users/{appId}/DeleteApp")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAppConfirmed(int id)
        {
            BlockedApp blockedApp = _db.BlockedApps.Find(id);
            _db.BlockedApps.Remove(blockedApp);
            _db.SaveChanges();
            return RedirectToAction("Users");
        }

    }
}