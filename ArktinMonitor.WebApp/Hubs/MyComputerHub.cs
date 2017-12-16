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
    public class MyComputerHub : Hub
    {
        public void Fart(int id, string text, string lang)
        {

            Clients.Group($"{Context.User.Identity.Name}:{id}").fart(text, lang);
        }

        public void JoinToGroup(int id)
        {
           
            Groups.Add(Context.ConnectionId, $"{Context.User.Identity.Name}:{id}");
        }

        public void Ping(int id)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").ping(Context.ConnectionId);
        }

        public void Pong(int id, string connectionId)
        {
            Clients.Client(connectionId).pong(id);
        }
        public override Task OnConnected()
        {
            //var name = Context.User.Identity.Name;
            //Groups.Add(Context.ConnectionId, name);

            //var connectionId = Context.ConnectionId;
            //var user = Context.User.Identity.Name;
            ////Clients.All.addNewMessageToPage(connectionId, "connected");
            //Clients.All.addNewMessageToPage(Context.User.Identity.Name, "connected");
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