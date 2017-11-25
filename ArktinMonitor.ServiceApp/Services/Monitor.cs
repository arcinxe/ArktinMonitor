namespace ArktinMonitor.ServiceApp.Services
{
    internal static class Monitor
    {
        public static void Run()
        {
            //IntervalTimeLogger.Start();
            ActionsManager.Start();
            Scheduler.Start();
        }

        public static void Stop()
        {
            //IntervalTimeLogger.Stop();
            Scheduler.Stop();
            ActionsManager.Stop();
        }
    }
}