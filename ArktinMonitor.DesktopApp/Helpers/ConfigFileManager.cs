using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.DesktopApp.Helpers
{
    internal static class ConfigFileManager
    {
        public static void PrepareConfigDataFile(string apiUrl, string path)
        {
            var configDataFile = JsonHelper.DeserializeJson<ConfigData>(path);
            if (configDataFile == null)
            {
                JsonHelper.SerializeToJsonFile(path, new ConfigData()
                {
                    ApiUrl = apiUrl,
                    DarkMode = true,
                });
            }
        }
    }
}