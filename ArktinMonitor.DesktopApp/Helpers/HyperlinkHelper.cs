using System.Diagnostics;

namespace ArktinMonitor.DesktopApp.Helpers
{
    internal static class HyperlinkHelper
    {
        /// <summary>
        /// Opens url in browser
        /// </summary>
        /// <param name="url">URL address</param>
        public static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo(url));
        } 
    }
}
