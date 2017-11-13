using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArktinMonitor.Helpers
{
    public class ServerClient
    {
        public HttpResponseMessage PostToServer(string url, string route, object model, string bearerToken = "")
        {
            using (var client = new HttpClient())
            {
                if (bearerToken != "")
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var response = client.PostAsJsonAsync($"{url}/{route}", model).Result;
                return response;
            }
        }

        public HttpResponseMessage GetFromServer(string url, string route,  string bearerToken = "")
        {
            using (var client = new HttpClient())
            {
                if (bearerToken != "")
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var response = client.GetAsync($"{url}/{route}").Result;
                return response;
            }
        }
    }
}
 