using System;
using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.ServiceApp
{
    internal static class Settings
    {
        //public static readonly string ApiUrl = /*"http://arktin.azurewebsites.net"*/"http://localhost:14100" /*"https://localhost:44368/"*/;

        public static readonly string ExecutablesPath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string DataStoragePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Arktin");

        public const string ApiUrl = "http://localhost:14100/";
        //public static readonly string ApiUrl = JsonHelper
        //    .DeserializeJson<ConfigData>(Path.Combine(ExecutablesPath, "ArktinMonitorData.an"))?.ApiUrl ?? "http://arktin.ml/";

        public const string TempBearerToken =
                "88NLnf7pZtiOip4IfQddKX2AP1bV5MgpVgZeNePnciGgopuzj-XpWEtwimx8ZHcRMmwsWJPuavJC1fasqtHbOjb0n7AmWarUzqZJ3iFr4FIf4AHW-TpwXjWvixx3odom2plgbFuftpggj6HcZ_5hPCSOOY3ZMhxyriKNfR9fKAB6perYSD1Clf0iNQtkPfEs21HUjl9HYaYGHLyzhrdM11CmgQc8UKg3AL2fj6Ji1zjQfZMsACyu30PMeNYZODO8KHnWtTPMOZ4RFQDN0y0ENREJS_Mms66MOgWp7uXU-kRIY5Ud_vM50iUragPWAxTOeRJp8MAkNyNDgydfFR1TgVim59pc83JCdKS0s6mwBnsM5xSPRtMg-So7MLguLcdhWYuE4hLeORfNx47JVYL6TY3K4G9pHa9nvwiRZ_uQ3hH3OCDJH1VIJKBQdf18Rsy55GaxbnwfLnvISP2Uri-vbTmE8NdI7wcnpx4kKZfIPZZsa3fXuHTqwySite9O8K75"
            ;

        public const int AppKillIntervalInSeconds = 55;
        public const int HardwareUpdateIntervalInSeconds = 5;
        public const int DisksUpdateIntervalInSeconds = 55;
        public const int ComputerUserChangesUpdaterIntervalInSeconds = 30;
        public const int SiteBlockerUpdaterIntervalInSeconds = 60;
        public const int SyncIntervalInSeconds = 30;
        public const int LogTimeIntervalInSeconds = 10;
    }
}