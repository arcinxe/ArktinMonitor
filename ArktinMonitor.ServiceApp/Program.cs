using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Services;
using System;
using System.IO;
using System.ServiceProcess;
using Topshelf;

namespace ArktinMonitor.ServiceApp
{
    internal static class Program
    {
        private static void Main()
        {
            //if (!UacHelper.IsElevated() && !Settings.PortableMode)
            //{
            //    Console.WriteLine("This app needs elevation in order to work!");
            //    Console.ReadLine();
            //    return;
            //}
            Directory.CreateDirectory(Settings.SystemRelatedStoragePath);
            LocalLogger.Append = true;
            LocalLogger.FileName = "service.log";
            LocalLogger.StoragePath = Settings.SystemRelatedStoragePath;
            IMyServiceContract myServiceObject = new ServiceMontior();


            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.Service<IMyServiceContract>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(name => myServiceObject);
                    serviceConfigurator.WhenStarted(sw => sw.Start());
                    serviceConfigurator.WhenStopped(sw => sw.Stop());
                    serviceConfigurator.WhenSessionChanged((csm, hc, chg) => csm.SessionChanged(chg));
                });

                hostConfigurator.EnableSessionChanged();

                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.SetDescription("Monitor and control this PC via web browser");
                hostConfigurator.SetDisplayName("ArktinMonitor Service");
                hostConfigurator.SetServiceName("ArktinMonitorService");
                hostConfigurator.SetInstanceName("ArktinMonitorService");


                hostConfigurator.StartAutomatically();

                hostConfigurator.EnableServiceRecovery(r =>
                {
                    r.OnCrashOnly();
                    r.RestartService(1); ////first
                    r.RestartService(1); ////second
                    r.RestartService(1); ////subsequents
                    r.SetResetPeriod(0);
                });

                hostConfigurator.DependsOnEventLog(); // Windows Event Log

                hostConfigurator.EnableShutdown();
                hostConfigurator.EnablePauseAndContinue();
                hostConfigurator.EnableShutdown();

                hostConfigurator.OnException(ex =>
                {
                    /* Log the exception */
                    /* not seen, I have a log4net logger here */
                });
            });
        }
    }
}