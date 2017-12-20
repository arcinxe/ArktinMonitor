using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.ServiceApp
{
    internal static class Settings
    {
        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly bool PortableMode = bool.Parse(ConfigurationManager.AppSettings["PortableMode"]);

        public static readonly string UserRelatedStoragePath = PortableMode ? ExecutablesPath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Arktin");

        public static readonly string SystemRelatedStoragePath = PortableMode ? ExecutablesPath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Arktin");


        public static readonly string ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];

        public static readonly int AppKillIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["AppKillIntervalInSeconds"]);
        public static readonly int HardwareUpdateIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["HardwareUpdateIntervalInSeconds"]);
        public static readonly int DisksUpdateIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["DisksUpdateIntervalInSeconds"]);
        public static readonly int UserChangesUpdaterIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["UserChangesUpdaterIntervalInSeconds"]);
        public static readonly int SiteBlockerUpdaterIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["SiteBlockerUpdaterIntervalInSeconds"]);
        public static readonly int SyncIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["SyncIntervalInSeconds"]);
        public static readonly int LogTimeIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["LogTimeIntervalInSeconds"]);
        public static readonly int HubStateCheckIntervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["HubStateCheckIntervalInSeconds"]);
    }
}