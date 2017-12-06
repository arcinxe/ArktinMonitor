using ArktinMonitor.DesktopApp.Helpers;
using ArktinMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;
using System.IO;
using System.Net;
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
            LocalLogger.StoragePath = Settings.UserRelatedStoragePath;
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
            LocalLogger.StoragePath = Settings.SystemRelatedStoragePath;
            ConfigFileManager.PrepareConfigDataFile(Settings.ApiUrl, Path.Combine(Settings.SystemRelatedStoragePath, "ArktinMonitorData.an"));
      
            var credentials = new CredentialsManager(Settings.ApiUrl,Settings.UserRelatedStoragePath,Settings.SystemRelatedStoragePath,"ArktinMonitor");
            var access = credentials.TryGetWebApiToken();
        
            if (access)
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