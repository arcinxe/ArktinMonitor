using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkitnMonitor.DesktopApp
{
    internal static class Settings
    {
        public const string ApiUrl = "http://arktin.ml/";

        public static bool LogFullExceptions = true;
        public static readonly string LocalStoragePath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;
    }
}
