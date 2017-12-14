using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;

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
            var computers = db.Computers/*.Where(c => c.WebAccount.Email == User.Identity.Name)*/.ToList();

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
            var computer = db.Computers.FirstOrDefault(c => c.ComputerId == computerId /*&& c.WebAccount.Email == User.Identity.Name*/);
            var viewModel = computer.ToViewModel(
                db.Disks.Where(d => d.ComputerId == computer.ComputerId).ToList(),
                 db.ComputerUsers.Where(u => u.ComputerId == computer.ComputerId).ToList(),
                 db.LogTimeIntervals.Where(l => l.ComputerId == computer.ComputerId && l.StartTime.Day==DateTime.Now.Day && l.StartTime.Month == DateTime.Now.Month && l.StartTime.Year == DateTime.Now.Year).ToList());

            if (computer == null) return View("Error");

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