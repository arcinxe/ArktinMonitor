using ArktinMonitor.Data.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace ArktinMonitor.ServiceApp.Helpers
{
    public static class ComputerHelper
    {
        public static ComputerLocal GetComputer()
        {
            var computer = new ComputerLocal()
            {
                Cpu = GetCpuName().Replace("(tm)", "™").Replace("(R)", "®").Replace("(TM)", "™").Replace("(C)", "©"),
                Gpu = GetGpuName(),
                Name = Environment.MachineName,
                Ram = GetTotalRamInGigaBytes(),
                OperatingSystem = GetWindowsName(),
                MacAddress = GetMacAddress(),
                Disks = GetDisks(),
                ComputerUsers = ComputerUsersHelper.GetComputerUsers(),
                LogTimeIntervals = new List<LogTimeIntervalLocal>()
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
            var result = "";

            var results = gpuNames.OrderByDescending(r => r[1]).FirstOrDefault();
            if (results != null)
                result = results[0];

            return result;
        }

        public static double GetTotalRamInGigaBytes()
        {
            var rams = GetComponent("Win32_PhysicalMemory").Get();
            var totalMemory = 0d;

            foreach (var ram in rams)
            {
                totalMemory += Convert.ToDouble(ram.GetPropertyValue("capacity"));
            }
            //     Bytes         KB     MB     GB
            return totalMemory / 1024 / 1024 / 1024;
        }

        public static string GetMacAddress()
        {
            var mac = NetworkInterface.GetAllNetworkInterfaces()
             .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
             .Select(nic => nic.GetPhysicalAddress()
             .ToString()).FirstOrDefault();

            var macAddress = string.Empty;
            if (mac == null) return macAddress;

            for (var i = 0; i < mac.Length; i++)
            {
                if (i % 2 == 0 && i != 0) macAddress += ':';
                var character = mac[i];
                macAddress += character;
            }
            return macAddress;
        }

        public static List<DiskLocal> GetDisks()
        {
            var disks = new List<DiskLocal>();
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.IsReady)
                {
                    disks.Add(new DiskLocal()
                    {
                        Letter = driveInfo.Name,
                        Name = driveInfo.VolumeLabel,
                        TotalSpaceInGigaBytes = (double)driveInfo.TotalSize / 1024 / 1024 / 1024,
                        FreeSpaceInGigaBytes = (double)driveInfo.AvailableFreeSpace / 1024 / 1024 / 1024
                    });
                }
            }
            return disks;
        }

        // Returns list of devices of querred hardware class.
        // https://msdn.microsoft.com/en-us/library/aa389273(v=vs.85).aspx
        private static ManagementObjectSearcher GetComponent(string hwclass)
        {
            return new ManagementObjectSearcher($"select * from {hwclass}");
        }

        public static string GetWindowsName()
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