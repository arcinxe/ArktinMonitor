using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.ServiceApp.Helpers;
using ArktinMonitor.ServiceApp.Services;

namespace ArktinMonitor.ServiceApp
{
    internal class Monitor
    {
        private bool _continue;

        public void Run()
        {
            _continue = true;
            while (_continue)
            {
                LocalLogger.Log(ComputerUsersHelper.CurrentlyLoggedInUser());
                System.Threading.Thread.Sleep(5000);
            }
        }

        public void Stop()
        {
            _continue = false;
        }
    }
}
