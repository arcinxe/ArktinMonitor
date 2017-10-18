using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Models;
using Microsoft.VisualBasic.Devices;
using System.Management;
using Microsoft.Win32;

namespace ArktinMonitor.ConsoleClient.Helpers
{
    public static class ComputerHelper
    {
        public static Models.Computer GetComputer()
        {
            var computer = new Models.Computer()
            {
                Cpu = GetCpuName(),
                Gpu = GetGpuName(),
                Name = Environment.MachineName,
                Ram = GetTotalRamInBytes() / (1024 * 1024),
                OperatingSystem = FriendlyWindowsName()
            };
            return computer;
        }

        public static string GetCpuName()
        {
            var cpus = GetComponent("Win32_Processor").Get();
            var cpuNames = new List<string>();

            foreach (var cpu in cpus)
            {
                cpuNames.Add(cpu.GetPropertyValue("Name").ToString());
            }

            return cpuNames.FirstOrDefault();
        }

        public static string GetGpuName()
        {
            var gpus = GetComponent("Win32_VideoController").Get();
            var gpuNames = new List<string[]>();

            foreach (var gpu in gpus)
            {
                gpuNames.Add(new[]
                {
                    gpu.GetPropertyValue("Name").ToString(),
                    gpu.GetPropertyValue("AdapterRAM").ToString()
                });
            }

            return gpuNames.OrderByDescending(r => r[1]).FirstOrDefault()[0];
        }

        public static double GetTotalRamInBytes()
        {
            return new ComputerInfo().TotalPhysicalMemory;

        }


        // Returns list of devices of querred hardware class.
        // https://msdn.microsoft.com/en-us/library/aa389273(v=vs.85).aspx
        private static ManagementObjectSearcher GetComponent(string hwclass)
        {
            return new ManagementObjectSearcher($"select * from {hwclass}");
        }

        public static string FriendlyWindowsName()
        {
            var productName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            var csdVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (productName != "")
            {
                return (productName.StartsWith("Microsoft") ? "" : "Microsoft ") + productName +
                            (csdVersion != "" ? " " + csdVersion : "");
            }
            return "";
        }
        
        private static string HKLM_GetString(string path, string key)
        {
            try
            {
                var rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }
    }
}
