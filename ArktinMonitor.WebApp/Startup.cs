using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArktinMonitor.WebApp.Startup))]
namespace ArktinMonitor.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
