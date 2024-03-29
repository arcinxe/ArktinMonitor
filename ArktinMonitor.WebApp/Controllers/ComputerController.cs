﻿using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ArktinMonitor.WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class ComputerController : ApiController
    {
        private readonly ArktinMonitorDataAccess _db = new ArktinMonitorDataAccess();

        [Route("Computers")]
        [HttpGet]
        public IHttpActionResult GetAllComputers()
        {
            var computers = _db.Computers.Where(c => c.WebAccount.Email == User.Identity.Name)
                            .AsEnumerable()
                            .Select(c => c.ToResourceModel()).ToList();
            return Ok(computers);
        }

        [Route("Computers")]
        [HttpPost]
        public IHttpActionResult UpdateComputer(ComputerResourceModel computer)
        {
            var account = _db.WebAccounts
                .FirstOrDefault(wa => wa.Email == User.Identity.Name);
            if (account == null) return NotFound();

            var exist = computer.ComputerId != 0 && _db.Computers.Any(c => c.ComputerId == computer.ComputerId);
            if (!exist)
            {
                var oldComputer =
                    _db.Computers.AsNoTracking().FirstOrDefault(c => c.Name == computer.Name && c.WebAccount.Email == User.Identity.Name);
                if (oldComputer != null)
                {
                    exist = true;
                    computer.ComputerId = oldComputer.ComputerId;
                }
            }
            var computerModel = computer.ToModel();

            computerModel.WebAccountId = account.WebAccountId;
            if (exist)
            {
                _db.Entry(computerModel).State = EntityState.Modified;
            }
            else
            {
                _db.Computers.Add(computerModel);
            }
            _db.SaveChanges();
            return Ok(computerModel.ComputerId);
        }
    }
}