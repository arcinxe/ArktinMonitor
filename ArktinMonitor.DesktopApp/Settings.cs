using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.DesktopApp
{
    internal static class Settings
    {
        //public const string ApiUrl = "http://localhost:14100/";
        public const string ApiUrl = "http://arktin.ml/";

        public static bool LogFullExceptions = true;
        public static readonly string LocalStoragePath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;
    }
}
