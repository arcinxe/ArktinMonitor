using System;
using System.Diagnostics;
using System.IO;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp
{
    internal static class Settings
    {
        //public static readonly string ApiUrl = /*"http://arktin.azurewebsites.net"*/"http://localhost:14100" /*"https://localhost:44368/"*/;

        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string DataStoragePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Arktin");

        public const string ApiUrl = "http://arktin.ml/";
        //public static readonly string ApiUrl = JsonHelper
        //    .DeserializeJson<ConfigData>(Path.Combine(ExecutablesPath, "ArktinMonitorData.an"))?.ApiUrl ?? "http://arktin.ml/";



        public const int AppKillIntervalInSeconds = 5;
        public const int HardwareUpdateIntervalInSeconds = 30;
        public const int DisksUpdateIntervalInSeconds = 15;
        public const int ComputerUserChangesUpdaterIntervalInSeconds = 60;
        public const int SiteBlockerUpdaterIntervalInSeconds = 60;
        public const int SyncIntervalInSeconds = 60;
        public const int LogTimeIntervalInSeconds = 30;

    }
}
