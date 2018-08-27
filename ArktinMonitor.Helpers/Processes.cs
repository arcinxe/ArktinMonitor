using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Helpers
{
    public static class Processes
    {
        public static IEnumerable<BasicProcess> GetProcesses()
        {
            const string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process";
            var searcher = new ManagementObjectSearcher(wmiQueryString);
            var results = searcher.Get();

            var query = from p in Process.GetProcesses()
                        join mo in results.Cast<ManagementObject>()
                        on p.Id equals (int)(uint)mo["ProcessId"]
                        //				where (string)mo["ExecutablePath"] != null
                        select new BasicProcess()
                        {
                            ProcessId = p.Id,
                            Path = (string)mo["ExecutablePath"] ?? "[NoData]",
                            Name = p.ProcessName,
                            Session = p.SessionId
                        };
            return query.ToList();
        }

        public static IEnumerable<BasicProcess> GetProcesses(string name) => GetProcesses()
            .Where(p => p.Name.ToLower() == name.ToLower());

        public static BasicProcess GetProcess(int processId) => GetProcesses()
            .FirstOrDefault(p => p.ProcessId == processId);

        public static bool RunApp(string executable, string arguments = null)
        {
            try
            {
                Process.Start(executable, arguments);
                return false;
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(RunApp), e);
                return true;
            }
        }

        public static ProcessDetails GetProcessDetails(int processId)
        {
            var process = Process.GetProcessById(processId);
            try
            {
                return new ProcessDetails()
                {
                    WindowTitle = process.MainWindowTitle,
                    ProcessId = processId,
                    Description = Process.GetProcessById(processId).MainModule.FileVersionInfo.FileDescription,
                    FileName = Process.GetProcessById(processId).MainModule.FileName,
                    LegalCopyright = Process.GetProcessById(processId).MainModule.FileVersionInfo.LegalCopyright
                };
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(GetProcessDetails), e);
                return new ProcessDetails();
            }
        }

        public class BasicProcess
        {
            public int ProcessId { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public int Session { get; set; }
        }

        public class ProcessDetails
        {
            public int ProcessId { get; set; }
            public string Description { get; set; }
            public string WindowTitle { get; set; }
            public string FileName { get; set; }
            public string LegalCopyright { get; set; }
        }
    }
}
