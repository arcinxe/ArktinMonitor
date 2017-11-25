using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ArktinMonitor.Helpers
{
    public class ServerClient
    {
        public HttpResponseMessage PostToServer(string url, string route, object model, string bearerToken = "")
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (bearerToken != "")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    var response = client.PostAsJsonAsync($"{url}/{route}", model).Result;
                    return response;
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(PostToServer), e);
                return null;
            }
        }

        public HttpResponseMessage GetFromServer(string url, string route, string bearerToken = "")
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (bearerToken != "")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    var response = client.GetAsync($"{url}/{route}").Result;
                    return response;
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(GetFromServer), e);
                return null;
            }
        }
    }
}