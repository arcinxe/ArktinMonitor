using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class AppsBlocker
    {
        public static void StartAppKiller(List<BlockedApplicationLocal> blockedApplications)
        {
            foreach (var process in GetProcesses())
            {
                if (blockedApplications.Any(app => app.Path == process.Path))
                {
                    Process.GetProcessById(process.ProcessId).Kill();
                }
            }

                GetProcesses()
                    .Where(p => blockedApplications.Any(app => app.Path == p.Path)).ToList()
                    .ForEach(p => Process.GetProcessById(p.ProcessId));
        }

        private static List<BasicProcess> GetProcesses()
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
