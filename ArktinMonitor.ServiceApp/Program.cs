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
            if (!UacHelper.IsElevated() && !Settings.PortableMode)
            {
                Console.WriteLine("This app needs elevation in order to work!");
                Console.ReadLine();
                return;
            }
            Directory.CreateDirectory(Settings.SystemRelatedStoragePath);
            LocalLogger.Append = true;
            LocalLogger.FileName = "service.log";
            LocalLogger.StoragePath = Settings.SystemRelatedStoragePath;

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