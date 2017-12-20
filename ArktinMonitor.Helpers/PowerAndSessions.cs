using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ArktinMonitor.Helpers
{
    public static class PowerAndSessions
    {
        [DllImport("user32")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        [DllImport("user32")]
        private static extern void LockWorkStation();
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        public static void Shutdown(int seconds)
        {
            Process.Start("shutdown", $"/s /t {seconds}");
        }

        public static void Restart()
        {
            Process.Start("shutdown", "/r /t 1");
        }

        public static void LogOut()
        {
            ExitWindowsEx(0, 0);
        }

        public static void Lock()
        {
            LockWorkStation();
        }

        public static void Hibernate()
        {
            SetSuspendState(true, true, true);
        }

        public static void Sleep()
        {
            SetSuspendState(false, true, true);
        }
    }
}
