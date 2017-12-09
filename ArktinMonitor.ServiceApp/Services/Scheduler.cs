using ArktinMonitor.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class Scheduler
    {
        private static bool _active;
        private static Task _task;
        //private static volatile int _seconds;
        //private static Timer _timer;

        public static void Start()
        {
            _active = true;
            //_timer = new Timer(IncrementSeconds, null, 1000, 1000);
            _task = Task.Run(() => Run());
        }

        public static void Stop()
        {
            _active = false;
            //_timer.Change(Timeout.Infinite, Timeout.Infinite);
            //_timer.Dispose();
            _task.Wait();
            _task.Dispose();
        }

        private static void Run()
        {
            var seconds = 0;
            while (_active)
            {
                Thread.Sleep(1000);
                if (seconds % Settings.LogTimeIntervalInSeconds == 0)
                {
                    ActionsManager.EnqueuNewAction(IntervalTimeLogger.Run);
                }

                if (seconds % Settings.HardwareUpdateIntervalInSeconds == 0)
                {
                    ActionsManager.EnqueuNewAction(DataUpdateManager.UpdateComputer);
                }

                if (seconds % Settings.DisksUpdateIntervalInSeconds == 0)
                {
                    ActionsManager.EnqueuNewAction(DataUpdateManager.UpdateDisks);
                }

                if (seconds % Settings.UserChangesUpdaterIntervalInSeconds == 0)
                {
                    ActionsManager.EnqueuNewAction(DataUpdateManager.UpdateUsers);
                }

                if (seconds % Settings.AppKillIntervalInSeconds == 0 && !Settings.PortableMode)
                {
                    ActionsManager.EnqueuNewAction(AppsBlocker.StartAppKiller);
                }

                if (seconds % Settings.SiteBlockerUpdaterIntervalInSeconds == 0 && !Settings.PortableMode)
                {
                    ActionsManager.EnqueuNewAction(SitesBlocker.BlockSites);
                }

                if (seconds % Settings.SyncIntervalInSeconds == 0)
                {
                    ActionsManager.EnqueuNewAction(SyncManager.SyncData);
                }

                //LocalLogger.Log();
                seconds++;
                // TEMP
                if (seconds % 5 == 0)
                {
                   


                }
                //ActionsManager.EnqueuNewAction(() => LocalLogger.Log( ComputerUsersHelper.CurrentlyLoggedInUser()));
                //LocalLogger.Log($"Scheduler is running. Seconds: {seconds}. {DateTime.Now-start:ss\\s\\:fff\\m\\s}");
            }
        }

        //private static void IncrementSeconds(object state)
        //{
        //    _seconds++;
        //}
    }
}