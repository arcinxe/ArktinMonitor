using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class AppsBlocker
    {
        public static void StartAppKiller()
        {
            LocalLogger.Log($"Method {nameof(StartAppKiller)} is running");

            //if (!SessionManager.Unlocked) return;
            var user = JsonLocalDatabase.Instance.Computer.ComputerUsers
                .FirstOrDefault(u => u.Name == ComputerUsersHelper.CurrentlyLoggedInUser());
            if (user == null) return;
            var processes = GetProcesses();
            foreach (var process in processes)
            {
                if (user.BlockedApplications.Where(a => a.Active).All(app => app.Path != process.Path)) continue;
                try
                {
                    Process.GetProcessById(process.ProcessId).Kill();
                    LocalLogger.Log($"App {process.Path} with PID {process.ProcessId} has been closed!");
                }
                catch (Exception e)
                {
                    LocalLogger.Log("AppBlocker", e);
                }
            }
        }

        private static IEnumerable<BasicProcess> GetProcesses()
        {
            const string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process";
            var searcher = new ManagementObjectSearcher(wmiQueryString);
            var results = searcher.Get();

            var query = from p in Process.GetProcesses()
                        join mo in results.Cast<ManagementObject>()
                        on p.Id equals (int)(uint)mo["ProcessId"]
                        where (string)mo["ExecutablePath"] != null
                        select new BasicProcess()
                        {
                            ProcessId = p.Id,
                            Path = (string)mo["ExecutablePath"]
                        };
            return query.ToList();
        }

        private class BasicProcess
        {
            public int ProcessId { get; set; }

            public string Path { get; set; }
        }
    }
}
