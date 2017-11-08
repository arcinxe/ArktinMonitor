using System;
using System.ServiceProcess;
using ArktinMonitor.ServiceApp.Services;

namespace ArktinMonitor.ServiceApp
{
    internal static class Program
    {
        private static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Start console app
                new Monitor().Run();
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
