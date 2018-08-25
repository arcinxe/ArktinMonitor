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
        public void Fart(int id, string text, string lang)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").fart(text, lang);
        }

        public void SendMessageToCurrentUser(string id, string text)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").sendMessageToCurrentUser(text);
        }
        public void PowerAction(int id, string actionName, int delayInSeconds)
        {
            Clients.Group($"{Context.User.Identity.Name}:{id}").powerAction(actionName, delayInSeconds);
        }

        public void RequestProcessList(int id)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
            Clients.Group(groupName).getProcesses();
        }

        public void SendProcessesToPage(int id, List<Processes.BasicProcess> processes)
        {
            var groupName = $"{Context.User.Identity.Name}:{id}";
                Clients.Group(groupName).displayProcesses(processes);
//            LogDataOnPage(id,processes.FirstOrDefault()?.Name);
        }

        //public void RequestScreenShot(int id)
        //{
        //    var groupName = $"{Context.User.Identity.Name}:{id}";
        //    Clients.Group(groupName).captureScreen();
        //}

        //public void SendImageToPage(int id, string base64Image)
        //{
        //    var groupName = $"{Context.User.Identity.Name}:{id}";
        //    Clients.Group(groupName).displayImageOnPage(base64Image);
        //}

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
            Clients.Client(connectionId).pong(id);
            Clients.Client(connectionId).logOnPage("Computer connected!");
        }
        public override Task OnConnected()
        {
            //Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
            //Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"connected@{Context.Request.Headers["User-Agent"]}, with id: {Context.ConnectionId}");
            Clients.Client(Context.User.Identity.Name)
                .logOnPage($"User@{Context.Request.Headers["User-Agent"]} connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"disconnected@{Context.Request.Headers["User - Agent"]}, with id: {Context.ConnectionId}");
            Clients.Group(Context.User.Identity.Name)
                .logOnPage($"User@{Context.Request.Headers["User-Agent"]} disconnected");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //Clients.Group(Context.User.Identity.Name)
            //    .logOnPage($"User@{Context.Request.Headers["User-Agent"]} reconnected");
            Clients.All.addNewMessageToPage(Context.User.Identity.Name, $"reconnected@{Context.Request.Headers["User-Agent"]}, with id: {Context.ConnectionId}");

            return base.OnReconnected();
        }
    }
}