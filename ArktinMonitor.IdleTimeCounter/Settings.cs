using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.IdleTimeCounter
{
    internal static class Settings
    {
        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly bool PortableMode = bool.Parse(ConfigurationManager.AppSettings["PortableMode"]);

        public static readonly string UserRelatedStoragePath = PortableMode ? ExecutablesPath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Arktin");
    }
}