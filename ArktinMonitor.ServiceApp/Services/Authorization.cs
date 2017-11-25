using ArktinMonitor.Data.Models;
using System.Net.Http;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class Authorization
    {
        public static TokenResponse GetBearerToken { get; private set; } = null;

        public static void RenewBearerToken(string username, string password)
        {
            var body = new StringContent($"username={username}&password={password}&grant_type=password");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Settings.ApiUrl + "/token", body).Result;
                var content = response.Content.ReadAsAsync<TokenResponse>().Result;
                GetBearerToken = content;
            }
        }
    }
}