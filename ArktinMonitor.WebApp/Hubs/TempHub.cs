using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ArktinMonitor.Data.Models;
using ArktinMonitor.WebApp.Providers;
using Microsoft.AspNet.SignalR;

namespace ArktinMonitor.WebApp.Hubs
{
    [Authorize]
    public class TempHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        //[SwitchableAuthorization]
        [Authorize]
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage($"{name} ({Context.User.Identity.Name})", message);
        }

        public override Task OnConnected()
        {

                //var connectionId = Context.ConnectionId;
                //var user = Context.User.Identity.Name;
                ////Clients.All.addNewMessageToPage(connectionId, "connected");
                Clients.All.addNewMessageToPage(Context.User.Identity.Name, "connected");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, "disconnected");

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, "reconnected");
            return base.OnReconnected();
        }
    }
}