using ArktinMonitor.Helpers;
using System;
using System.IO;

namespace ArktinMonitor.IdleTimeCounter
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var dataStoragePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Arktin");
                LocalLogger.StoragePath = dataStoragePath;
                LocalLogger.FileName = "IdleTimeCounter.log";
                Directory.CreateDirectory(dataStoragePath);

                var idleTickCount = LastUserInput.GetIdleTickCount();
                LocalLogger.Log($"Idle time: {idleTickCount / 10000}");
                using (var streamWriter = new StreamWriter(Path.Combine(dataStoragePath, "IdleTime.an"), false))
                {
                    streamWriter.WriteLine(idleTickCount);
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(IdleTimeCounter), e);
            }
        }
    }
}