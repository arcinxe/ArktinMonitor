using System;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class Monitor
    {
        public static void Run()
        {
            LocalLogger.Log("Service has been started!");
            ActionsManager.Start();
            Scheduler.Start();
        }

        public static void Stop()
        {
            LocalLogger.Log("Stopping service");
            HubService.LogOnPage("Stopping service!");
            Scheduler.Stop();
            ActionsManager.Stop();
            HubService.Stop();
            SessionManager.KillIdleTimeCounters();
            SitesBlocker.ClearHostsFile();
            LocalLogger.Log("Service has been stopped!");
        }
    }
}