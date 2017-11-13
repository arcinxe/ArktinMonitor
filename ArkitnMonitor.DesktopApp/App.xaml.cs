using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ArkitnMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;

namespace ArkitnMonitor.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            base.OnStartup(e);
        }

        private static void Current_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LocalLogger.Log("Global exception",e.Exception);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            //CredentialsManager.PurgePassword();
            if (CredentialsManager.AreCredentialsStored())
            {
                var editorWindow = new EditorWindow();
                editorWindow.Show();
            }
            else
            {
                var logInWindow = new LoginWindow();
                logInWindow.Show();
            }
        }
    }
}
