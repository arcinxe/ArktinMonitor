using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Models
{
    public class ArktinMonitorContext : DbContext
    {
        public DbSet<ComputerUser> Users { get; set; }
        public DbSet<WebAccount> WebAccounts { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Disk> Disks { get; set; }
        public DbSet<BlockedApplication> BlockedApplications { get; set; }
        public DbSet<BlockedSite> BlicBlockedSites { get; set; }
        public DbSet<LogTimeInterval> LogTimeIntervals { get; set; }
    }
}
