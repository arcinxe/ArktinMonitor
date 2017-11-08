using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Timers;
using ArktinMonitor.Data.Models;
using ArktinMonitor.ServiceApp.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    internal class Monitor
    {
        private Timer _timer;

        public void Run()
        {
            DoIt(null, null);
            _timer = new Timer(5000);
            _timer.Elapsed += DoIt;
            _timer.Start();


            //CredentialsManager.SavePassword(new NetworkCredential("Arktin", Console.ReadLine()).SecurePassword);
            ////Console.WriteLine(new NetworkCredential("Arktin",CredentialsManager.GetPassword()).Password);
            //var blockedSites = new List<BlockedSiteLocal>()
            //{
            //    new BlockedSiteLocal(){Name = "9gag", UrlAddress = "9gag.com"},
            //    new BlockedSiteLocal(){Name = "reddit", UrlAddress = "reddit.com"},
            //    new BlockedSiteLocal() { Name = "4chan", UrlAddress = "4chan.org" }
            //};
            //SitesBlocker.BlockSites(blockedSites);
            //SitesBlocker.ClearHostsFile();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void DoIt(object sender, ElapsedEventArgs e)
        {

            try
            {
                //DataUpdateManager.UpdateComputer(ComputerHelper.GetComputer());
                //DataUpdateManager.UpdateDisks(ComputerHelper.GetDisks());
                //DataUpdateManager.UpdateUsers(ComputerUsersHelper.GetComputerUsers());
            }
            catch (Exception exception)
            {
                LocalLogger.Log("Monitor", exception);
            }

            //LocalLogger.Log(ComputerUsersHelper.CurrentlyLoggedInUser());
            //var computer = JsonHelper.DeserializeJson<ComputerLocal>(Path.Combine(Settings.LocalPath, "database.json"));
            //JsonHelper.SerializeToJsonFile(Path.Combine(Settings.LocalPath, "database2.json"), computer);
        }
    }
}
