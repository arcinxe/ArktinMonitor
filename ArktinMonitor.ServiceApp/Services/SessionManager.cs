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

        /// <summary>
        /// Checks amount of time that past since last user interaction (use mouse or keyboard)
        /// </summary>
        /// <returns>Returns TimeSpan of the  time that past since last user interaction</returns>
        public static TimeSpan GetIdleTime()
        {
            var username = GetActive();
            var filePath = Settings.PortableMode 
                ? Path.Combine(Settings.ExecutablesPath, "IdleTime.an") 
                : username == null ? null : $@"C:\Users\{username}\AppData\Local\Arktin\IdleTime.an";
            try
            {
                if (Environment.UserInteractive)
                {
                    using (var p = new Process())
                    {
                        p.StartInfo = new ProcessStartInfo(Path.Combine(Settings.ExecutablesPath,
                            "ArktinMonitor.IdleTimeCounter.exe")) /*{Verb = "runas"}*/;
                        p.Start();
                        p.WaitForExit();
                    }
                }
                else
                {
                    ExecuteHelper.StartProcessAsCurrentUser($"{Path.Combine(Settings.ExecutablesPath, "ArktinMonitor.IdleTimeCounter.exe")}");
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log("IdleTimeCounter", e);
            }
            var idleTimeInMiliSeconds = JsonHelper.DeserializeJson<long>(filePath);
            var result = new TimeSpan(idleTimeInMiliSeconds * 10000);
            LocalLogger.Log($"Idle time: {result:mm\\m\\:ss\\s\\:ff\\m\\s}");
            return result;
        }
    }
}