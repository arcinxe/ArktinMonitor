using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.UserSessionWorker
{
    internal static class Settings
    {
        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string UserRelatedStoragePath = /*PortableMode ? ExecutablesPath : */Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Arktin");
    }
}