using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.ConsoleClient
{
    internal static class Settings
    {
        public static string ApiUrl = "http://localhost:14100";// https://localhost:44368/

        public static string LocalPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }
}
