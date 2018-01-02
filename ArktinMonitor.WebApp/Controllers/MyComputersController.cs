using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using ArktinMonitor.WebApp.ViewModels;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("MyComputers")]
    public class MyComputersController : Controller
    {
        // GET: MyComputers
        public ActionResult Index()
        {
            var db = new ArktinMonitorDataAccess();
            var computers = db.Computers.Where(c => c.WebAccount.Email == User.Identity.Name).ToList();

            var viewModel = computers
                .Select(c => c.ToViewModel(
                    db.Disks.Where(d => d.ComputerId == c.ComputerId).ToList(),
                    db.ComputerUsers.Where(u => u.ComputerId == c.ComputerId).ToList(),
                    db.LogTimeIntervals.Where(l => l.ComputerId == c.ComputerId).ToList()
                    ));
            return View(viewModel.ToList());
        }

        // Get: MyComputers
        [Route("Details/{computerId}")]
        public ActionResult Details(int computerId)
        {
            var db = new ArktinMonitorDataAccess();
            var computer = db.Computers.FirstOrDefault(c => c.ComputerId == computerId && c.WebAccount.Email == User.Identity.Name);
            
            if (computer == null) return View("Error");
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone).Date.AddHours(-1);
            //var today = DateTime.Now.AddHours(1).Date;
            var endTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,easternZone).Date.AddDays(1).AddTicks(-1).AddHours(-1);
            //var endTime = DateTime.Now.AddHours(1).Date.AddDays(1).AddTicks(-1);

            var viewModel = computer.ToViewModel(
                db.Disks.Where(d => d.ComputerId == computer.ComputerId && !d.Removed).ToList(),
                 db.ComputerUsers.Where(u => u.ComputerId == computer.ComputerId).ToList(),
                 db.LogTimeIntervals.Where(l => l.ComputerId == computer.ComputerId && l.StartTime >= today && l.StartTime <= endTime).ToList());

            ViewBag.ComputerUserId = new SelectList(db.ComputerUsers, "ComputerUserId", "Name");
            return View(viewModel);
        }

        [AllowAnonymous]
        [Route("Temp")]
        public ActionResult TempAction()
        {
            return View();
        }

        [Route("Chat")]
        public ActionResult Chat()
        {
            return View();
        }




    }
}