using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.DesktopApp
{
    internal static class Settings
    {
        public static readonly string ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];

        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly bool PortableMode = bool.Parse(ConfigurationManager.AppSettings["PortableMode"]);

        public static readonly string UserRelatedStoragePath = PortableMode ? ExecutablesPath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Arktin");

        public static readonly string SystemRelatedStoragePath = PortableMode ? ExecutablesPath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Arktin");


    }
}