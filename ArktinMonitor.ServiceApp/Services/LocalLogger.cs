using System;
using System.IO;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class LocalLogger
    {
        public static void Log(string data = "")
        {
            LogText(data);
        }

        public static void Log(string sender, Exception exception)
        {
            LogException(sender, exception);
        }
        private static void LogText(string data)
        {
            // For better formationg when used without parameter.
            string separator = data == "" ? "" : "-";

            // If isn't running as a service then also writes data to the console.
            if (Environment.UserInteractive)
            {
                Console.WriteLine(Format(data, separator));
            }

            using (var sw = new StreamWriter(Path.Combine(Settings.LocalStoragePath, "log.log"), true))
            {
                sw.WriteLine(Format(data, separator));
            }
        }

        private static void LogException(string sender, Exception e)
        {
            Log($"[{sender}] -- Exception occured --");
            if (Settings.LogFullExceptions)
            {
                Log($"[{sender}] " + e.ToString());
            }
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
            return DateTime.Now.ToString() + " " + separator + " " + text;
        }
    }
}
