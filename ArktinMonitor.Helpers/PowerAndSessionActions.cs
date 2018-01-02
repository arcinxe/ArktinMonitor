using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ArktinMonitor.Helpers
{
    public static class PowerAndSessionActions
    {
        [DllImport("user32")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        [DllImport("user32")]
        private static extern void LockWorkStation();
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        public static void Shutdown(int seconds = 1)
        {
            LocalLogger.Log($"Method {nameof(Shutdown)} called!");
            Process.Start("shutdown", $"/s /f /t {seconds}");
        }

        public static void Restart(int seconds = 1)
        {
            LocalLogger.Log($"Method {nameof(Restart)} called!");
            Process.Start("shutdown", $"/r /f /t {seconds}");
        }

        public static void LogOut()
        {
            LocalLogger.Log($"Method {nameof(LogOut)} called!");
            ExitWindowsEx(0, 0);
        }

        public static void Lock()
        {
            LocalLogger.Log($"Method {nameof(Lock)} called!");
            LockWorkStation();
        }

        public static void Hibernate()
        {
            LocalLogger.Log($"Method {nameof(Hibernate)} called!");
            SetSuspendState(true, true, true);
        }

        public static void Sleep()
        {
            LocalLogger.Log($"Method {nameof(Sleep)} called!");
            SetSuspendState(false, true, true);
        }
    }
}
