using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Services;
using System;
using System.IO;
using System.ServiceProcess;

namespace ArktinMonitor.ServiceApp
{
    internal static class Program
    {
        private static void Main()
        {
            Directory.CreateDirectory(Settings.DataStoragePath);
            LocalLogger.FileName = "service.log";
            LocalLogger.StoragePath = Settings.DataStoragePath;

            if (Environment.UserInteractive)
            {
                // Start console app
                Monitor.Run();
                Console.WriteLine("Press Enter key to exit...");
                Console.ReadLine();
            }
            else
            {
                ServiceBase.Run(new MonitorService());
            }
        }
    }
}