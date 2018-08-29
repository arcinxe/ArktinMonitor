using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Services;
using Topshelf;

namespace ArktinMonitor.ServiceApp
{
   public class ServiceMontior : IMyServiceContract
    {
        public void Start()
        {
            try
            {
                System.IO.File.Create(@"D:\start.txt");
                Monitor.Run();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Stop()
        {
            try
            {
                System.IO.File.Create(@"D:\stop.txt");
                Monitor.Stop();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void SessionChanged(SessionChangedArguments e)
        {
            var unlocked = e.ReasonCode == SessionChangeReasonCode.ConsoleConnect ||
                           e.ReasonCode == SessionChangeReasonCode.RemoteConnect ||
                           e.ReasonCode == SessionChangeReasonCode.SessionLogon ||
                           e.ReasonCode == SessionChangeReasonCode.SessionRemoteControl ||
                           e.ReasonCode == SessionChangeReasonCode.SessionUnlock;
                //changeDescription.Reason == SessionChangeReason.SessionLogon ||
                  //         changeDescription.Reason == SessionChangeReason.SessionUnlock;
            SessionManager.Unlocked = unlocked;
            LocalLogger.Log($"{e.ReasonCode}. Unlocked: {unlocked}");
            HubService.LogOnPage($"Session changed to {e.ReasonCode}");
        }
    }
}
