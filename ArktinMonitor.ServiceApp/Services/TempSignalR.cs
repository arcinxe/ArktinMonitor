using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ArktinMonitor.ServiceApp.Services
{
    // https://stackoverflow.com/questions/26657296/signalr-authentication-with-webapi-bearer-token
    #region StackOverflow

    //You need to configure your signalr like this;

    //app.Map("/signalr", map =>
    //{
    //map.UseCors(CorsOptions.AllowAll);
    //map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
    //{
    //    Provider = new QueryStringOAuthBearerProvider()
    //});

    //var hubConfiguration = new HubConfiguration
    //{
    //    Resolver = GlobalHost.DependencyResolver,
    //};
    //map.RunSignalR(hubConfiguration);
    //});
    //Then you need to write a basic custom OAuthBearerAuthenticationProvider for signalR which accepts access_token as query string.

    //public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    //{
    //    public override Task RequestToken(OAuthRequestTokenContext context)
    //    {
    //        var value = context.Request.Query.Get("access_token");

    //        if (!string.IsNullOrEmpty(value))
    //        {
    //            context.Token = value;
    //        }

    //        return Task.FromResult<object>(null);
    //    }
    //}
    //After this all you need is to send access_token with signalr connection as querystring.

    //$.connection.hub.qs = { 'access_token': token };
    //And for your hub just ordinary[Authorize] attribute

    //public class impAuthHub : Hub
    //{
    //[Authorize]
    //public void SendMessage(string name, string message)
    //{
    //Clients.All.newMessage(name, message);
    //}
    //}

    #endregion

    /// <summary>
    /// After few fucking days of researching this shit is finally working :D
    /// </summary>
    public static class TempSignalR
    {
        public static void Start()
        {
            var hubConnection = new HubConnection("https://arktin.azurewebsites.net/");
            //var cookieToken = "HdR3ISBq8HLaczEbn_gXcJfn_MhlNoXZ6ydUoJiJbfx1-i5vtGvBHFlXT0kiACLPFQRj8O_O1XSeQwh-4OmypHKg9FUYrt3frhYXu1k956IODyRNW4hfa7K-e33YcTJtg0begPvEWeS92I0OcdbU-5-8RU0wTira_tUsJhis-o1It7gOv6lJHx_7ssjq7pzM2OaKOlA-3ir9oN40C6V5WrQEsjxEeiuSBaVaGakejvDCwOnivUUr0NQ40ABuDP-KXeElnRfYUxx_xu3OUgO3uj7vmqc2moRnrI7oDQo7mCHlWNwI4icQ4rroNdXBJaJVyayaSTZj7TktnAFCIJmeRGdBv96p0CQlqQf7iquT4KqfT2GrgnUnUhL5othFhe7CUBIKmbWwIGu-rFU-7lE70y6HciaSqxBnJqdvu_jpj8H85_QfkIYUNNzIXoTOSadkXBhKLOtOBauaJ8F5yCvaZwea4uj14hW_feDw-BKSQ7kjMsQDjhWkUCkY6_PR7vpM";
            const string bearerToken = "7KWzUFpTmXjYPHJf45R9XKkoEGrtanwgiCLzfBoPDOaFivZ0HFpuF0I5j1X0KYfYywCBPbxVjDOdN6UD3UP5fPE4mguU41E3C6HvGHgFS4XQMOOfpcfOSwQEklBLBnIYbs-XcDt2U2o-KaiDYpsAuCVq6va8lqd_QBLiCYrWkbllYScFNnG05zPgsZ6N27mYyEUNhAxnaLXBou7QR9EsxEqHjJLLKYPxcWNHMmjRF8BiufEW8f711Zaf8eX3m7_WKyR6RRF65vI7spEX8JW2-RDs9iI-rjC8Q696mWlp5XJp3R0o3WyVHA47rm0X83dezBtAZj6AkahnYlJVWhWVn_nmSKX-ROdZ32UBVNhYsP5kNJICCIMdVt5ycU2qb_NC3dGt1_yRu0aktHOISw_ZAsvRIhCz6BmP9842VBauDDXdkIdnLb5OZgvFZR0lLeZFE6EdnqI4Q8aNuqfE1VeG_fx44ybODYkXAQbMdeVCYX0"; //"9AmuGmqFhoyXXU3lDNuxrIyml70WvmFSZkLCmQCE47R7h2AU0Z7R91KtXgqr3sJ5U61wkcJF_WFdnJE4VHbTiQxPYcCplQzfTBMZG-jtj_4MGgnndDBIlSC9wEXJLQWhxQeCAoT5EQ9KF4_i9AKbsUMiFscmDEl7PBitaBJeIfSV0GLMnY2_AfTBYbP72iP9n39QJkJEM6mmfCur4Oy1Elv9g3XAPsMXnANkWP6o_d-AwhP5aGtrODp5PbJCwF6iJE5mALbrj6BaK5VAc3YgsLwqXpM_bNCYHJrNBssq8l4QhtbWLBS_UDSgne0swg041wvtLIGjnqJsrtFn-IND4iXmRJ6prXRB1kmg8gI_XJm1ZxxgT9lYT-IJj7SPMMQZAuQebjxFMwduTcSAP8ISIa4CUyoT47uBPAl7nOg_Ee7K2kyOunDUBIvjPK2HsJZsu1YuDgEMURlBob32SNXj0025BhgG6xLWZjddWtTebx5ft61l6ReR4gBO6nBUpYSs";

            //var cookie = new Cookie("ArktinMonitorCookie", token2){Domain = "arktin.azurewebsites.net" };
            //hubConnection.CookieContainer = new CookieContainer();
            //hubConnection.CookieContainer.Add(cookie);
            hubConnection.Headers.Add("Authorization", "Bearer " + bearerToken);
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            IHubProxy myHubProxy = hubConnection.CreateHubProxy("TempHub");

            myHubProxy.On<string, string>("addNewMessageToPage",
                (name, message) => Console.Write($"{name} - {message}\n"));

            hubConnection.Start().Wait();
            //System.Threading.Thread.Sleep(5000);

            while (true)
            {
                //System.Threading.Thread.Sleep(1000);

                myHubProxy.Invoke("Send", "service", Console.ReadLine()).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        if (task.Exception != null)
                            Console.WriteLine("There was an error opening the connection:{0}",
                                task.Exception.GetBaseException());
                    }
                }).Wait();

            }
        }
    }
}
