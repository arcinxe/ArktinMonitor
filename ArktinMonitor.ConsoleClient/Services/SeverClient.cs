using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ArktinMonitor.ConsoleClient.Services
{
    public class ServerClient
    {
        public async Task<HttpResponseMessage> SendToServer(string route, object model, string bearerToken = "")
        {
            
            using (var client = new HttpClient())
            {
                if (bearerToken != "")
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var response = await client.PostAsJsonAsync(Settings.ApiUrl + "/" + route, model);
                return response;
            }
        }
    }
}
