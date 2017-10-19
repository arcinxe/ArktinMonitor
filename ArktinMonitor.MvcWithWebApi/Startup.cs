using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArktinMonitor.MvcWithWebApi.Startup))]
namespace ArktinMonitor.MvcWithWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
