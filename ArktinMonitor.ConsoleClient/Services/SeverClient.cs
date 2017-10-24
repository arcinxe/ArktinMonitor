using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ArktinMonitor.ConsoleClient.Services
{
    public class ServerClient
    {
        public async Task<HttpResponseMessage> SendToServer(string route, object model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(Settings.ApiUrl + "/" + route, model);
                //bool returnValue = await response.Content.ReadAsAsync<bool>();
                return response;
            }
        }
    }
}
