﻿using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class SitesBlocker
    {
        private static readonly string HostsFileLocation = Path
            .Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");

        public static void BlockSites()
        {
            LocalLogger.Log($"Method {nameof(BlockSites)} is running");
            //if (!SessionManager.Unlocked) return;
            var user = JsonLocalDatabase.Instance.Computer?.ComputerUsers?
                .FirstOrDefault(u => u.Name == ComputerUsersHelper.CurrentlyLoggedInUser());
            if (user == null) return;
            var blockedSites = user.BlockedSites;
            ClearHostsFile();

            var blockedEntries = new StringBuilder();

            var hostsFileContent = File.ReadAllText(HostsFileLocation);

            foreach (var blockedSite in blockedSites.Where(s => s.Active))
            {
                blockedEntries.AppendLine($"0.0.0.0 {blockedSite.UrlAddress} www.{blockedSite.UrlAddress} #Autogenerated by ArktinMonitor");
            }
            File.WriteAllText(HostsFileLocation, hostsFileContent + Environment.NewLine + blockedEntries);
        }

        public static void ClearHostsFile()
        {
            var hostsFileContent = new StringBuilder();

            var rawList = File.ReadAllLines(HostsFileLocation).ToList();
            foreach (var item in rawList)
            {
                if (item.Contains("#Autogenerated by ArktinMonitor")) continue;
                hostsFileContent.Append(item + Environment.NewLine);
            }
            File.WriteAllText(HostsFileLocation, hostsFileContent.ToString().TrimEnd());
        }
    }
}