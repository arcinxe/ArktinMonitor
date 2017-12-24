using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;
using ArktinMonitor.WebApp.Providers;
using Microsoft.AspNet.SignalR;

namespace ArktinMonitor.WebApp.Hubs
{
    [Authorize]
    public class MyComputerHub : Hub
    {
        private readonly ComputerGroups _computerGroups = new ComputerGroups();
        public void Fart(int id, string text, string lang)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").fart(text, lang);
        }

        public void JoinToGroup(int id)
        {
            try
            {
                var groupName = $"{Context.User.Identity.Name}:{id}";
                if (_computerGroups.GetGroupName(Context.ConnectionId) == groupName) return;
                Groups.Add(Context.ConnectionId, groupName);
                _computerGroups.JoinToGroup(Context.ConnectionId, groupName);
            }
            catch (Exception e)
            {
                var db = new ArktinMonitorDataAccess();
                db.DebugLogs.Add(new DebugLog(){Message = $"Exception occurred \n{e}", TimeStamp = DateTime.Now});
                db.SaveChanges();
            }
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
            //Clients.All.addNewMessageToPage(connectionId, "connected");
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, "connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var groupName = _computerGroups.GetGroupName(Context.ConnectionId);
            _computerGroups.LeaveGroup(Context.ConnectionId, groupName);
            Groups.Remove(Context.ConnectionId, groupName);
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, "disconnected");

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, "reconnected");

            return base.OnReconnected();
        }

        private class ComputerGroups
        {
            public List<ComputerGroup> Groups { get; set; } = new List<ComputerGroup>();

            public string GetGroupName(string connectionId)
            {
                return Groups.FirstOrDefault(g => g.Users.Any(u => u.ConnectionId == connectionId))?.Name;
            }

            public void JoinToGroup(string connectionId, string groupName)
            {
                if (Groups.All(g => g.Name != groupName))
                {
                    Groups.Add(new ComputerGroup { Name = groupName, Users = new List<ComputerGroupUser>() });
                }
                var group = Groups.FirstOrDefault(g => g.Name == groupName);
                group?.Users.Add(new ComputerGroupUser() { ConnectionId = connectionId, GroupName = groupName });
            }

            public void LeaveGroup(string connectionId, string groupName)
            {
                var group = Groups.FirstOrDefault(g => g.Name == groupName);
                group?.Users.RemoveAll(g => g.ConnectionId == connectionId);
                if (group?.Users.Count == 0) Groups.Remove(group);
            }
            public class ComputerGroupUser
            {
                public string ConnectionId { get; set; }
                public string GroupName { get; set; }
                public string Role { get; set; }
            }

            internal class ComputerGroup
            {
                public string Name { get; set; }

                public List<ComputerGroupUser> Users { get; set; }
            }

        }
    }
}