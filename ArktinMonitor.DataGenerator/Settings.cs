using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.DataGenerator
{
    internal static class Settings
    {
        public static int AmountOfWebAccounts = 50;
        public const int MaxAmountOfCOmputers = 6;
        public const int MaxAmountOfComputerUsers = 8;
        public const int MaxAmountOfDisks = 5;
        public const int MaxAmountOfBlockedApps = 4;
        public const int MaxAmountOfBlockedSites = 2;
        public const int MaxAmountOfLogTimeIntervals = 25;

        public static readonly string LocalStoragePath = Environment.UserInteractive
       ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
       : AppDomain.CurrentDomain.BaseDirectory;
    }
}