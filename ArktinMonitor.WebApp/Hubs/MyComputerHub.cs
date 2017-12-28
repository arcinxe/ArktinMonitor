using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ArktinMonitor.WebApp.Hubs
{
    [Authorize]
    public class MyComputerHub : Hub
    {
        //private static readonly ConcurrentDictionary<string, List<string>> Map =
            //new ConcurrentDictionary<string, List<string>>();
        //private static readonly List<ConnectionAndGroup> ConnectionsAndGroups = new List<ConnectionAndGroup>();
        //private static readonly ComputerGroups MyComputerGroups = new ComputerGroups();
        public void Fart(int id, string text, string lang)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").fart(text, lang);
        }

        //public void GetGroupname()
        //{
        //    //var existingGroupName = MyComputerGroups.GetGroupName(Context.ConnectionId);
        //    var existingGroupName = ConnectionsAndGroups.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId)?.GroupName;
        //    Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"GroupName = {existingGroupName}");
        //}
        public void SendMessageToCurrentUser(string id, string text)
        {
             Clients.Group($"{Context.User.Identity.Name}:{id}").sendMessageToCurrentUser(text);
        }
        public void PowerAction(int id, string actionName, int delayInSeconds)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").powerAction(actionName, delayInSeconds);
            }
        public void JoinToGroup(int id)
        {

            var newGroupName = $"{Context.User.Identity.Name}:{id}";
            //if (ConnectionsAndGroups.Any(c => c.GroupName == newGroupName && c.ConnectionId == Context.ConnectionId))
            //{
            //    return;
            //}

            //var groups = ConnectionsAndGroups.Where(c => c.GroupName == newGroupName);
            //foreach (var group in groups)
            //{
            //    try
            //    {
            //        Groups.Remove(group.ConnectionId, group.GroupName);
            //    }
            //    catch (Exception)
            //    {
            //        // ignored
            //    }
            //    ConnectionsAndGroups.RemoveAll(c =>
            //        c.GroupName == group.GroupName && c.ConnectionId == group.ConnectionId);
            //}
            Groups.Add(Context.ConnectionId, newGroupName);
            //ConnectionsAndGroups.Add(new ConnectionAndGroup(){ConnectionId = Context.ConnectionId, GroupName = newGroupName});
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
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"connected@{Context.Request.Headers["User-Agent"]}, with id: {Context.ConnectionId}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //var groupName = MyComputerGroups.GetGroupName(Context.ConnectionId);
            //MyComputerGroups.LeaveGroup(Context.ConnectionId, groupName);
            //if (string.IsNullOrWhiteSpace(groupName)) return base.OnDisconnected(stopCalled);
            //Groups.Remove(Context.ConnectionId, groupName);
            //var connectionId = Context.ConnectionId;
            //var groups = ConnectionsAndGroups.Where(c => c.ConnectionId == connectionId);
            //foreach (var group in groups)
            //{
            //    try
            //    {
            //        Groups.Remove(connectionId, group.GroupName);
            //    }
            //    catch (Exception)
            //    {
            //        // ignored
            //    }
            //    ConnectionsAndGroups.RemoveAll(c => c.ConnectionId == connectionId && c.GroupName == group.GroupName);
            //}
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"disconnected@{Context.Request.Headers["User - Agent"]}, with id: {Context.ConnectionId}");

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"reconnected@{Context.Request.Headers["User-Agent"]}, with id: {Context.ConnectionId}");

            return base.OnReconnected();
        }

        //private class ComputerGroups
        //{
        //    public List<ComputerGroup> Groups { get; set; } = new List<ComputerGroup>();

        //    public string GetGroupName(string connectionId)
        //    {
        //        return Groups.FirstOrDefault(g => g.Users.Any(u => u.ConnectionId == connectionId))?.Name;
        //    }

        //    public void JoinToGroup(string connectionId, string groupName)
        //    {
        //        if (Groups.All(g => g.Name != groupName))
        //        {
        //            Groups.Add(new ComputerGroup { Name = groupName, Users = new List<ComputerGroupUser>() });
        //        }
        //        var group = Groups.FirstOrDefault(g => g.Name == groupName);
        //        group?.Users.Add(new ComputerGroupUser() { ConnectionId = connectionId, GroupName = groupName });
        //    }

        //    public void LeaveGroup(string connectionId, string groupName)
        //    {
        //        var group = Groups.FirstOrDefault(g => g.Name == groupName);
        //        group?.Users.RemoveAll(g => g.ConnectionId == connectionId);
        //        if (group?.Users.Count == 0) Groups.Remove(group);
        //    }
        //    public class ComputerGroupUser
        //    {
        //        public string ConnectionId { get; set; }
        //        public string GroupName { get; set; }
        //        public string Role { get; set; }
        //    }

        //    internal class ComputerGroup
        //    {
        //        public string Name { get; set; }

        //        public List<ComputerGroupUser> Users { get; set; }
        //    }

        //}
        //public class ConnectionAndGroup
        //{
        //    public string ConnectionId { get; set; }

        //    public string GroupName { get; set; }
        //}
    }
}