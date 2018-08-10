using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ArktinMonitor.Helpers
{
    public static class LocalLogger
    {
        /// <summary>
        /// Target directory to store log file. By default location of the executable.
        /// </summary>
        public static string StoragePath = Environment.UserInteractive
             ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
             : AppDomain.CurrentDomain.BaseDirectory;

        private static readonly object Locker = new object();

        /// <summary>
        /// Name for log file
        /// </summary>
        public static string FileName = "log.log";

        /// <summary>
        /// Enables or disables LocalLogger
        /// </summary>
        public static bool Enabled = true;

        /// <summary>
        /// Determines whenever newest entries in log file should be at the top or the bottom.
        /// true appends new records, while false puts them at the top ofthe file, false makes all slower
        /// </summary>
        public static bool Append = false;

        /// <summary>
        /// Determines whenever log should be saved on disk or only showed in the console
        /// </summary>
        public static bool SaveOnDisk = true;

        public static void Log(string data = "")
        {
            if (!Enabled) return;
            LogText(data);
        }

        public static void Log(object obj)
        {
            //Log(obj.Dump());
            Log(JsonConvert.SerializeObject(obj, Formatting.Indented));
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
            lock (Locker)
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine(Format(data, separator));
                }

                if (!SaveOnDisk) return;
                try
                {
                    var timestamp = DateTime.Now;
                    if (Append)
                    {
                        using (var sw = new StreamWriter(Path.Combine(StoragePath, FileName + timestamp), true))
                        {
                            sw.WriteLine(Format(data, separator));
                        }
                    }
                    else
                    {
                        var currentContent = new StringBuilder();
                        try
                        {
                            var rawList = File.ReadAllLines(Path.Combine(StoragePath, FileName + timestamp)).ToList();
                            foreach (var item in rawList) 
                            { 
                                currentContent.Append(item + Environment.NewLine);
                            }
                        }
                        catch (Exception)
                        {
                            // ignored (file does not exist)
                        }
                        if (!Directory.Exists(StoragePath)) Directory.CreateDirectory(StoragePath);
                        File.WriteAllText(Path.Combine(StoragePath, FileName + timestamp), Format(data, separator) + Environment.NewLine + currentContent);
                    }

                }
                catch (Exception e)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";

                        eventLog.WriteEntry($"LocalLogger: {data}\n{e}", EventLogEntryType.Information, 101, 1);
                    }
                }
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