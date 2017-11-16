using System.Windows;
using ArktinMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LocalLogger.Append = false;
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
