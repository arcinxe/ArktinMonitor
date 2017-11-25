using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class Monitor
    {
       
        public static void Run()
        {
            IntervalTimeLogger.Start();
            ActionsManager.Start();
            Scheduler.Start();
        }

        public static void Stop()
        {
            IntervalTimeLogger.Stop();
            Scheduler.Stop();
            ActionsManager.Stop();
        }
    }
}
