using System;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Services;
using System.ServiceProcess;

namespace ArktinMonitor.ServiceApp
{
    public partial class MonitorService : ServiceBase
    {
        public MonitorService()
        {
            InitializeComponent();
            CanHandleSessionChangeEvent = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Monitor.Run();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void OnStop()
        {
            try
            {
                Monitor.Stop();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            var unlocked = changeDescription.Reason == SessionChangeReason.SessionLogon ||
                           changeDescription.Reason == SessionChangeReason.SessionUnlock;
            SessionManager.Unlocked = unlocked;
            LocalLogger.Log($"{changeDescription.Reason}. Unlocked: {unlocked}");
            HubService.LogOnPage($"Session changed to {changeDescription.Reason}");
        }
    }
}