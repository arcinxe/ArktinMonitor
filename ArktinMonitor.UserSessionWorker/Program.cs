using System.IO;
using ArktinMonitor.Helpers;
using System.Linq;
using System.Windows;

namespace ArktinMonitor.UserSessionWorker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //TextToSpeechHelper.VoiceDebug($"total arguments of {args.Length}.");// TODO: Remove this
            //TextToSpeechHelper.VoiceDebug($"first argument: {args.FirstOrDefault()}.");// And this
            //TextToSpeechHelper.VoiceDebug($"second argument: {args.ElementAtOrDefault(1)}.");// This too
            if (!args.Any()) return;
            switch (args.FirstOrDefault())
            {
                case "lock":
                    PowerAndSessionActions.Lock();
                    break;
                case "logout":
                    PowerAndSessionActions.LogOut();
                    break;
                case "message":
                    MessageBox.Show(args[1], "ArktinMonitor", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    //System.Windows.Forms.MessageBox.Show(args[1], "ArktinMonitor");
                    break;
                case "screen":
                    Directory.CreateDirectory(Settings.UserRelatedStoragePath);
                    var path = Path.Combine(Settings.UserRelatedStoragePath, "ss.an");
                    ScreenCapture.CaptureScreenToFile(path);
                    break;
                case "keys":
                    Helpers.KeySender.Test(args[1]);
                    break;
                case "run":
                    Helpers.Processes.RunApp(args[1], args.ElementAtOrDefault(2));
                    break;
            }
        }
    }
}
