using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.ServiceApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            if (Environment.UserInteractive)
            {
                
                new Monitor().Run();
                // Start console app
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
