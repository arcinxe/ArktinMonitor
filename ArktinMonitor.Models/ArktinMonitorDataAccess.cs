using System.Data.Entity;
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
        public ArktinMonitorDataAccess()
            : base("name=DefaultConnection")
        {
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