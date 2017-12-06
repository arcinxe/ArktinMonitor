using System;
using System.Net.Http;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.DesktopApp.Helpers
{
    public static class Authorization
    {
        public static TokenResponse GetBearerToken { get; private set; } = null;

        public static TokenResponse RenewBearerToken(string username, string password)
        {
            var body = new StringContent($"username={username}&password={password}&grant_type=password");
            System.Threading.Thread.Sleep(1000); // TODO: Remove later
            try
            {

                using (var client = new HttpClient())
                {
                    LocalLogger.Log("Authorization started");
                    var response = client.PostAsync(Settings.ApiUrl + "token", body).Result;
                    var content = response.Content.ReadAsAsync<TokenResponse>().Result;
                    LocalLogger.Log("Token: " + content.AccessToken);
                    GetBearerToken = content;
                    return content;
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log("Authorization", e);
                return null;
            }
        }
    }
}
