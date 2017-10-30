using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.ServiceApp
{
    internal static class Settings
    {
        public static readonly string ApiUrl = /*"http://arktin.azurewebsites.net"*/"http://localhost:14100" /*"https://localhost:44368/"*/;

        public static readonly string LocalPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public static readonly bool LogFullExceptions = true;

        public static readonly string LocalStoragePath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;
    }
}
