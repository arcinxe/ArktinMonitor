﻿using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ArktinMonitor.ServiceApp
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            new ServiceController(serviceInstaller.ServiceName).Start();
        }
    }
}