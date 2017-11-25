using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Services;

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
