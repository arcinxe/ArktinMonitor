using ArktinMonitor.DesktopApp.Helpers;
using ArktinMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;
using System.IO;
using System.Windows;

namespace ArktinMonitor.DesktopApp
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LocalLogger.FileName = "DesktopApp.log";
            LocalLogger.Append = false;
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            base.OnStartup(e);
        }

        private static void Current_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LocalLogger.Log("Global exception", e.Exception);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConfigFileManager.PrepareConfigDataFile(Settings.ApiUrl, Path.Combine(Settings.DataStoragePath, "ArktinMonitorData.an"));
            //CredentialsManager.PurgePassword();
            //if (CredentialsManager.AreCredentialsStored())
            if (CredentialsManager.CheckWebApiAccess(Settings.ApiUrl, Settings.DataStoragePath))
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