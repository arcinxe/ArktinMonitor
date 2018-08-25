using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class AppsBlocker
    {
        public static void StartAppKiller()
        {
            LocalLogger.Log($"Method {nameof(StartAppKiller)} is running");

            //if (!SessionManager.Unlocked) return;
            try
            {
                var user = JsonLocalDatabase.Instance.Computer.ComputerUsers?
                    .FirstOrDefault(u => u.Name == ComputerUsersHelper.CurrentlyLoggedInUser());
                if (user?.BlockedApps == null || user.BlockedApps.Count == 0) return;
                var processes = ArktinMonitor.Helpers.Processes.GetProcesses();
                var count = 0;
                foreach (var process in processes)
                {
                    if (user.BlockedApps.Where(a => a.Active).All(app => app.Path != process.Path)) continue;
                    try
                    {
                        Process.GetProcessById(process.ProcessId).Kill();
                        LocalLogger.Log($"App {process.Path} with PID {process.ProcessId} has been closed!");
                        count++;
                    }
                    catch (Exception e)
                    {
                        LocalLogger.Log("AppBlocker", e);
                    }
                }
                if (count > 0) HubService.LogOnPage($"Killed {count} apps!");
            }
            catch (Exception e)
            {
                LocalLogger.Log("AppBlocker", e);
            }
        }

      
    }
}