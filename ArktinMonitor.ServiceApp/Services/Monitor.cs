using System;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class Monitor
    {
        public static void Run()
        {
            //IntervalTimeLogger.Start();
                TempSignalR.Start();
            try
            {
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(TempSignalR), e);
            }
            //ActionsManager.Start();
            //Scheduler.Start();
        }

        public static void Stop()
        {
            //IntervalTimeLogger.Stop();
            Scheduler.Stop();
            ActionsManager.Stop();
        }
    }
}