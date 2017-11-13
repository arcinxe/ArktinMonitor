using System;
using System.Net.Http;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Helpers
{
    public static class Authorization
    {
        public static TokenResponse RenewBearerToken(string url, string username, string password)
        {
            var body = new StringContent($"username={username}&password={password}&grant_type=password");
            try
            {
                using (var client = new HttpClient())
                {
                    LocalLogger.Log("Authorization started");
                    var response = client.PostAsync(url + "token", body).Result;
                    var content = response.Content.ReadAsAsync<TokenResponse>().Result;
                    LocalLogger.Log("Token: " + content.AccessToken);
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
