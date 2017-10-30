using System.Linq;
using System.Web.Http;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;

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

        [Route("Computers", Name = "PostComputer")]
        [HttpPost]
        public IHttpActionResult AddComputer(ComputerResourceModel computer)
        {
            var exist = computer.ComputerId != 0 && _db.Computers.Any(c => c.ComputerId == computer.ComputerId);
            if (exist) return Ok();

            var computerModel = computer.ToModel();
            var account = _db.WebAccounts
                .FirstOrDefault(wa => wa.Email == User.Identity.Name);
            if (account == null) return NotFound();

            computerModel.WebAccountId = account.WebAccountId;
            _db.Computers.Add(computerModel);
            _db.SaveChanges();
            return CreatedAtRoute("PostComputer", new { id = computerModel.ComputerId }, computerModel);
        }
    }
}
