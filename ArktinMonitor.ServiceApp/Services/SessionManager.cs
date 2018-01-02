using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.ServiceApp.Services
{
    /// <summary>
    /// Represents state of computer and users
    /// </summary>
    internal static class SessionManager
    {
        /// <summary>
        /// Returns true if any User is signed in and session is unlocked
        /// </summary>
        public static bool Unlocked = true;

        /// <summary>
        /// Returns user that current session is unlocked or null if locked
        /// </summary>
        /// <returns></returns>
        public static string GetActive()
        {
            var user = ComputerUsersHelper.CurrentlyLoggedInUser();
            return Unlocked ? user : null;
        }

        public static string CurrentUserAppDataFolder
        {
            get
            {
                var username = GetActive();
                return username == null ? null : Path
                    .Combine(Path.GetPathRoot(Environment.SystemDirectory), $@"Users\{username}\AppData\Local\Arktin");
            }
        }

        /// <summary>
        /// Checks amount of time that past since last user interaction (use mouse or keyboard)
        /// </summary>
        /// <returns>Returns TimeSpan of the  time that past since last user interaction</returns>
        public static TimeSpan GetIdleTime()
        {
            if (CurrentUserAppDataFolder == null) return new TimeSpan();
            var filePath = Path.Combine(CurrentUserAppDataFolder, "IdleTime.an");
            try
            {
                var processes = Process.GetProcessesByName("ArktinMonitor.IdleTimeCounter");

                if (processes.Length != 1)
                {
                    foreach (var process in processes)
                    {
                        process.Kill();
                    }

                    if (Environment.UserInteractive)
                    {
                        using (var p = new Process())
                        {
                            p.StartInfo = new ProcessStartInfo(Path.Combine(Settings.ExecutablesPath,
                                "ArktinMonitor.IdleTimeCounter.exe")) /*{Verb = "runas"}*/;
                            p.Start();
                            //p.WaitForExit();
                        }
                    }
                    else
                    {
                        ExecuteHelper.StartProcessAsCurrentUser($"{Path.Combine(Settings.ExecutablesPath, "ArktinMonitor.IdleTimeCounter.exe")}");
                    }
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log("IdleTimeCounter", e);
                return new TimeSpan();
            }
            var idleTimeInMiliSeconds = JsonHelper.DeserializeJson<long>(filePath);
            var result = new TimeSpan(idleTimeInMiliSeconds * 10000);
            LocalLogger.Log($"Idle time: {result:mm\\m\\:ss\\s\\:ff\\m\\s}");
            return result;
        }

        public static void KillIdleTimeCounters()
        {
            var processes = Process.GetProcessesByName("ArktinMonitor.IdleTimeCounter");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        public static void LogOutCurrentUser()
        {
            CallAppInUsersSession("logout");
        }
        public static void DisconnectCurrentUser()
        {
            CallAppInUsersSession("lock");
        }
        public static void SendMessageToCurrentUser(string text)
        {
            CallAppInUsersSession($"message \"{text}\""); ;
            HubService.LogOnPage($"Text message received: \"{text}\"");
        }

        public static void CaptureScreenOfCurrentUser()
        {
            CallAppInUsersSession("screen");
            System.Threading.Thread.Sleep(100);
        }

        private static void CallAppInUsersSession(string parameter)
        {
            ExecuteHelper.StartProcessAsCurrentUser(
                Path.Combine(Settings.ExecutablesPath, "ArktinMonitor.UserSessionWorker.exe"), " " + parameter);
        }
    }
}