using ArktinMonitor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class ProcessManager
    {
        public static int KillProcessesByName(string name)
        {
            var processes = ArktinMonitor.Helpers.Processes.GetProcesses(name);
            var basicProcesses = processes as Processes.BasicProcess[] ?? processes.ToArray();
            foreach (var process in basicProcesses)
            {
                try
                {
//                    LocalLogger.Log($"Killing process: {process.Name} : {process.ProcessId}");
                    Process.GetProcessById(process.ProcessId).Kill();
                    HubService.LogOnPage($"Killed process: {process.Name} : {process.ProcessId}");
                }
                catch (Exception e)
                {
                    LocalLogger.Log(nameof(KillProcessesByName), e);
                }
            }
            return basicProcesses.Count();
        }

    }
}
