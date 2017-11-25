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
            Monitor.Run();
        }

        protected override void OnStop()
        {
            Monitor.Stop();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            var unlocked = changeDescription.Reason == SessionChangeReason.SessionLogon ||
                           changeDescription.Reason == SessionChangeReason.SessionUnlock;
            SessionManager.Unlocked = unlocked;
            LocalLogger.Log($"{changeDescription.Reason}. Unlocked: {unlocked}");
        }
    }
}