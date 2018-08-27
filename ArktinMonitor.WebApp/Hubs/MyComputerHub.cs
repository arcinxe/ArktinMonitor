using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;
using Microsoft.AspNet.SignalR;

namespace ArktinMonitor.WebApp.Hubs
{
    [Authorize]
    public class MyComputerHub : Hub
    {
        public void Fart(int id, string command, string attributes)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").command(command, attributes);
        }

        public void PowerAction(int id, string actionName, int delayInSeconds)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").powerAction(actionName, delayInSeconds);
        }

        public void RequestProcesses(int id)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").requestProcesses();
        }

        public void SendProcessesToPage(int id, List<Processes.BasicProcess> processes)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
            Clients.Group(groupName).displayProcesses(processes);
        }

        public void FillProcessDetails(int id, Processes.ProcessDetails processDetails)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
            Clients.Group(groupName).updateProcessDetails(processDetails);
        }

        public void LogDataOnPage(int id, string text)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
            Clients.Group(groupName).logOnPage(text);
        }

        public void JoinToGroup(int id)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void Ping(int id)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").ping(Context.ConnectionId);
        }

        public void Pong(int id, string connectionId)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
            Clients.Group(groupName).pong(id);
            Clients.Group(groupName).logOnPage("Computer connected!");
        }
        public override Task OnConnected()
        {
            Clients.Client(Context.User.Identity.Name)
                .logOnPage($"User@{Context.Request.Headers["User-Agent"]} connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.Group(Context.User.Identity.Name)
                 .logOnPage($"User@{Context.Request.Headers["User-Agent"]} disconnected");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Clients.Group(Context.User.Identity.Name)
                .logOnPage($"User@{Context.Request.Headers["User-Agent"]} reconnected");
            return base.OnReconnected();
        }
    }
}