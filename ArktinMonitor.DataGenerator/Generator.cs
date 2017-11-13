using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.DataGenerator
{
    internal static class Generator
    {
        private const int MaxAmountOfWebAccounts = 15;
        private const int MaxAmountOfCOmputers = 4;
        private const int MaxAmountOfComputerUsers = 6;
        private const int MaxAmountOfDisks=5;
        private const int MaxAmountOfBlockedApps =4;
        private const int MaxAmountOfBlockedSites =2;
        private const int MaxAmountOfLogTimeIntervals =10;


        public static List<WebAccount> GenerateWebAccounts()
        {
            return new List<WebAccount>();
        }

        public static List<Computer> GenerateComputers()
        {

            return  new List<Computer>();
        }

        public static List<Disk> GenerateDisks()
        {
            return new List<Disk>();
        }

        public static List<ComputerUser> GeneratComputerUsers()
        {
            return new List<ComputerUser>();
        }

        public static List<LogTimeInterval> GenerateLogoIntervals()
        {
            return new List<LogTimeInterval>();
        }

        public static List<BlockedApplication> GeneBlockedApplications()
        {
            return new List<BlockedApplication>();
        }

        public static List<BlockedSite> GenerateBlockedSites()
        {
            return new List<BlockedSite>();
        }
    }
}
