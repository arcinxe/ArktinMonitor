using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.ServiceApp.Services;

namespace ArktinMonitor.ServiceApp
{
    public partial class MonitorService : ServiceBase
    {
        private Monitor _monitor;
        

        public MonitorService()
        {
            InitializeComponent();
            CanHandleSessionChangeEvent = true;
        }

        protected override void OnStart(string[] args)
        {
            _monitor = new Monitor();
            new System.Threading.Thread(_monitor.Run).Start();
        }

        protected override void OnStop()
        {
            _monitor.Stop();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            LocalLogger.Log(changeDescription.Reason.ToString());
        }
    }
}
