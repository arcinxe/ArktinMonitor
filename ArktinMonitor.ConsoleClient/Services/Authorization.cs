using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Other;

namespace ArktinMonitor.ConsoleClient.Services
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
