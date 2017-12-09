using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartupAttribute(typeof(ArktinMonitor.WebApp.Startup))]

namespace ArktinMonitor.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always // Add this line to enable detail mode in release
            };
            ConfigureAuth(app);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.MapSignalR();
            //app.Map("/signalr", map =>
            //{
            //    map.UseCors(CorsOptions.AllowAll);

            //    map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            //    {
            //        Provider = new QueryStringOAuthBearerProvider()
            //    });

            //    var hubConfiguration = new HubConfiguration
            //    {
            //        Resolver = GlobalHost.DependencyResolver,
            //    };
            //    map.RunSignalR(hubConfiguration);
            //});
        }
    }
}