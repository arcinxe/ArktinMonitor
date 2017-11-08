using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkitnMonitor.DesktopApp.Helpers
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
