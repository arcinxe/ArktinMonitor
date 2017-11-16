using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data
{
    public class ArktinMonitorDataAccess : DbContext
    {
        // Your context has been configured to use a 'ArktinMonitorDataAccess' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ArktinMonitor.Models.ArktinMonitorDataAccess' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ArktinMonitorDataAccess' 
        // connection string in the application configuration file.
        public ArktinMonitorDataAccess(): base("name=DefaultConnection") {}

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        public DbSet<ComputerUser> ComputerUsers { get; set; }
        public DbSet<WebAccount> WebAccounts { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Disk> Disks { get; set; }
        public DbSet<BlockedApplication> BlockedApplications { get; set; }
        public DbSet<BlockedSite> BlicBlockedSites { get; set; }
        public DbSet<LogTimeInterval> LogTimeIntervals { get; set; }
    }
}