using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.DesktopApp
{
    internal static class Settings
    {
        //public const string ApiUrl = "http://localhost:14100/";
        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string DataStoragePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Arktin");

        //public const string ApiUrl = "http://arktin.ml/";
        public static readonly string ApiUrl = JsonHelper.DeserializeJson<ConfigData>(Path.Combine(DataStoragePath, "ArktinMonitorData.an"))?.ApiUrl
            ?? "http://arktin.ml/";
    }
}