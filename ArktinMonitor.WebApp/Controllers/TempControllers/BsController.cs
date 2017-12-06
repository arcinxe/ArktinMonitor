using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArktinMonitor.Data;

namespace ArktinMonitor.WebApp.Controllers.TempControllers
{
    public class BsController : Controller
    {
        // GET: Bs
        public ActionResult Index()
        {
            var db = new ArktinMonitorDataAccess();
            return View(db);
        }
    }
}