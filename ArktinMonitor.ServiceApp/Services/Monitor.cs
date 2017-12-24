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
            Scheduler.Stop();
            ActionsManager.Stop();
            HubService.Stop();
            LocalLogger.Log("Service has been stopped!");
        }
    }
}