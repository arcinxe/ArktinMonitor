using System;
using System.IO;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;
using Microsoft.AspNet.SignalR.Client;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class HubService
    {
        private static readonly HubConnection HubConnection = new HubConnection(Settings.ApiUrl);
        private static IHubProxy _myHubProxy;
        //private static bool _running;
        public static bool IsRunning()
        {
            return HubConnection.State != ConnectionState.Disconnected;
        }
        public static void Start()
        {
            LocalLogger.Log($"Method {nameof(HubService)} is running");


            try
            {

                var credentialsManager = new CredentialsManager(Settings.ApiUrl, Settings.UserRelatedStoragePath, Settings.SystemRelatedStoragePath, "ArktinMonitor");

                var bearerToken = credentialsManager.LoadJsonWebToken().AccessToken;
                if (!HubConnection.Headers.TryGetValue("Authorization", out var value) || value != "Bearer " + bearerToken)
                {
                    HubConnection.Headers.Add("Authorization", "Bearer " + bearerToken);
                }

                if (_myHubProxy == null)
                {
                    _myHubProxy = HubConnection.CreateHubProxy("MyComputerHub");
                    HubConnection.Reconnected += () => { LocalLogger.Log("Reconnected to hub"); };
                    //HubConnection.Closed += () => { LocalLogger.Log("Connection closed"); };
                    HubConnection.StateChanged += state => { LocalLogger.Log($"State of connection changed from {state.OldState} to {state.NewState}"); };

                    _myHubProxy.On<string, string>("addNewMessageToPage",
                        (name, message) => LocalLogger.Log($"{name} - {message}\n"));

                    _myHubProxy.On<string, string>("fart", (s, s1) =>
                    {
                        TextToSpeechHelper.Speak(s, s1);
                        TempAction(s);
                    });
                    _myHubProxy.On<string, int>("PowerAction", PowerAction);
                    _myHubProxy.On<string>("ping", Pong);
                }
                //myHubProxy.On("fart", () => Console.WriteLine("Purrrrrr!"));
                LocalLogger.Log("Starting hub connection");

                HubConnection.Start().Wait();


                LocalLogger.Log("Joining to group");
                JoinToGroup();
            }
            catch (Exception e)
            {
                //_running = false;
                LocalLogger.Log(nameof(Start), e);
                //HubConnection.Stop();
            }
            HubConnection.Error += exception =>
            {
                LocalLogger.Log(nameof(HubConnection.Error), exception);
                if (exception is System.TimeoutException)
                {
                    //HubConnection.Stop();

                }
                //_running = false;
            };
            //_running = true;
        }

        private static void JoinToGroup()
        {
            try
            {
                var computer = JsonLocalDatabase.Instance.Computer;
                if (computer.ComputerId != 0)
                {
                    _myHubProxy.Invoke("JoinToGroup", computer.ComputerId).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            if (task.Exception != null)
                                //LocalLogger.Log($"There was an error opening the connection {task.Exception.GetBaseException()}");
                                LocalLogger.Log("JoinToGroup", task.Exception);
                        }
                    }).Wait();
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(JoinToGroup), e);
            }
        }

        private static void Pong(string connectionId)
        {
            try
            {
                LocalLogger.Log("Pong");
                _myHubProxy.Invoke("Pong", JsonLocalDatabase.Instance.Computer.ComputerId, connectionId).ContinueWith(
                    task =>
                    {
                        if (task.IsFaulted)
                        {
                            if (task.Exception != null)
                                //Console.WriteLine("There was an error opening the connection:{0}",
                                //    task.Exception.GetBaseException());
                                LocalLogger.Log("Pong", task.Exception);
                        }
                    });

            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(HubService), e);
            }
        }

        private static void PowerAction(string nameOfAction, int delayInSeconds)
        {
            switch (nameOfAction)
            {
                case "lock":
                    SessionManager.DisconnectCurrentUser();
                    break;
                case "logoff":
                    SessionManager.LogOutCurrentUser();
                    break;
                case "shutdown":
                    PowerAndSessionActions.Shutdown(delayInSeconds);
                    break;
                case "restart":
                    PowerAndSessionActions.Restart();
                    break;
                case "hibernate":
                    PowerAndSessionActions.Hibernate();
                    break;
                case "sleep":
                    PowerAndSessionActions.Sleep();
                    break;
                default:
                    LocalLogger.Log("Unknown SignalR action has been called. " +
                    $"{nameof(PowerAction)}(string nameOfAction = \"{nameOfAction}\" int delay = {delayInSeconds})");
                    break;
            }
        }
        //private static void GroupName()
        //{
        //    try
        //    {
        //        LocalLogger.Log("GetGroupname");
        //        _myHubProxy.Invoke("GetGroupname").ContinueWith(
        //            task =>
        //            {
        //                if (task.IsFaulted)
        //                {
        //                    if (task.Exception != null)
        //                        //Console.WriteLine("There was an error opening the connection:{0}",
        //                        //    task.Exception.GetBaseException());
        //                        LocalLogger.Log("Pong", task.Exception);
        //                }
        //            });

        //    }
        //    catch (Exception e)
        //    {
        //        LocalLogger.Log(nameof(HubService), e);
        //    }
        //}
        public static void Stop()
        {
            Task.Run(() =>
            {
                HubConnection.Stop();
                LocalLogger.Log($"{nameof(HubService)} has been stopped!");
            });
        }

        public static void Reconnect()
        {
            HubConnection.Start();
        }

        private static void TempAction(string text)
        {
            Helpers.ExecuteHelper.StartProcessAsCurrentUser(
                Path.Combine(Settings.ExecutablesPath, "ArktinMonitor.IdleTimeCounter.exe"), " " + text);
        }
    }
}
