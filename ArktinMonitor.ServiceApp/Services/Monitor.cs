using System;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class Monitor
    {
        public static void Run()
        {
            //IntervalTimeLogger.Start();

            ActionsManager.Start();
            Scheduler.Start();
            HubService.Start();
        }

        public static void Stop()
        {
            //IntervalTimeLogger.Stop();
            HubService.Stop();
            Scheduler.Stop();
            ActionsManager.Stop();
        }
    }
}