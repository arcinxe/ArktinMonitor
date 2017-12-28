using ArktinMonitor.Helpers;
using System;
using System.IO;
using System.Linq;

namespace ArktinMonitor.IdleTimeCounter
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                while (true)
                {
                    try
                    {
                        LocalLogger.StoragePath = Settings.UserRelatedStoragePath;
                        LocalLogger.FileName = "IdleTimeCounter.log";
                        Directory.CreateDirectory(Settings.UserRelatedStoragePath);
                        var path = Path.Combine(Settings.UserRelatedStoragePath, "IdleTime.an");
                        var idleTickCount = LastUserInput.GetIdleTickCount();
                        LocalLogger.Log("Test IdleTimeCounter");
                        LocalLogger.Log($"Idle time: {idleTickCount / 10000}, path: {path}");
                        using (var streamWriter = new StreamWriter(path, false))
                        {
                            streamWriter.WriteLine(idleTickCount);
                        }
                    }
                    catch (Exception e)
                    {
                        LocalLogger.Log(nameof(IdleTimeCounter), e);
                    }
                    System.Threading.Thread.Sleep(60000);
                }
            }


            switch (args.FirstOrDefault())
            {
                case "lock":
                    PowerAndSessionActions.Lock();
                    break;
                case "logout":
                    PowerAndSessionActions.LogOut();
                    break;
                case "message":
                    System.Windows.Forms.MessageBox.Show(args[1]);
                    break;
            }
        }
    }
}