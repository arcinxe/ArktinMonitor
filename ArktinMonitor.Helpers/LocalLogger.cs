using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ArktinMonitor.Helpers
{
    public static class LocalLogger
    {
        private static readonly string LocalStoragePath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static bool Enabled = true;
        public static bool Append = false;
        public static bool SaveOnDisk = true;

        public static void Log(string data = "")
        {
            if (!Enabled) return;
            LogText(data);
        }

        public static void Log(string sender, Exception exception)
        {
            if (!Enabled) return;
            LogException(sender, exception);
        }
        private static void LogText(string data)
        {
            // For better formating when used without parameter.
            var separator = data == "" ? "" : "-";

            // If isn't running as a service then also writes data to the console.
            if (Environment.UserInteractive)
            {
                Console.WriteLine(Format(data, separator));
            }
            if(!SaveOnDisk) return;
            if (Append)
            {
                using (var sw = new StreamWriter(Path.Combine(LocalStoragePath, "log.log"), true))
                {
                    sw.WriteLine(Format(data, separator));
                }
            }
            else
            {
                var currentContent = new StringBuilder();
                try
                {
                    var rawList = File.ReadAllLines(Path.Combine(LocalStoragePath, "log.log")).ToList();
                    foreach (var item in rawList)
                    {
                        currentContent.Append(item + Environment.NewLine);
                    }
                }
                catch (Exception)
                {
                    // ignored (file does not exist)
                }
                File.WriteAllText(Path.Combine(LocalStoragePath, "log.log"), Format(data, separator) + Environment.NewLine + currentContent);
            }


        }

        private static void LogException(string sender, Exception e)
        {
            Log($"[{sender}] -- Exception occured --");

            Log($"[{sender}] " + e);

            Log($"[{sender}] " + e.Message);

            // Recursively print exception's message.
            while (e.InnerException != null)
            {
                Log($"[{sender}] " + e.Message);
                e = e.InnerException;
            }
            // Blank line at the end.
            Log();
        }

        private static string Format(string text, string separator)
        {
            return DateTime.Now + " " + separator + " " + text;
        }
    }
}
