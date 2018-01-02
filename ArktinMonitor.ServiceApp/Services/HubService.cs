using ArktinMonitor.Helpers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class HubService
    {
        private static readonly HubConnection HubConnection = new HubConnection(Settings.ApiUrl);

        private static IHubProxy _myHubProxy;

        public static bool IsRunning()
        {
            return HubConnection.State != ConnectionState.Disconnected;
        }

        public static void Start()
        {
            LocalLogger.Log($"Method {nameof(HubService)} is running");
            try
            {
                var credentialsManager = new CredentialsManager(Settings.ApiUrl, Settings.UserRelatedStoragePath,
                    Settings.SystemRelatedStoragePath, "ArktinMonitor");

                var bearerToken = credentialsManager.LoadJsonWebToken().AccessToken;
                if (!HubConnection.Headers.TryGetValue("Authorization", out var value) ||
                    value != "Bearer " + bearerToken)
                {
                    HubConnection.Headers.Add("Authorization", "Bearer " + bearerToken);
                }

                if (_myHubProxy == null)
                {
                    _myHubProxy = HubConnection.CreateHubProxy("MyComputerHub");
                    HubConnection.StateChanged += state =>
                    {
                        LocalLogger.Log(
                            $"State of connection to hub changed from {state.OldState} to {state.NewState}");
                    };

                    _myHubProxy.On<string, string>("addNewMessageToPage",
                        (name, message) => LocalLogger.Log($"{name} - {message}\n"));

                    _myHubProxy.On<string, string>("fart", Speak);
                    _myHubProxy.On<string>("getInstalledVoices", GetInstalledVoicesList);
                    _myHubProxy.On<string>("sendMessageToCurrentUser", SessionManager.SendMessageToCurrentUser);
                    _myHubProxy.On<string, int>("powerAction", PowerAction);
                    //_myHubProxy.On("captureScreen", CaptureScreen);
                    _myHubProxy.On<string>("ping", Pong);
                }
                LocalLogger.Log("Starting hub connection");

                HubConnection.Start().Wait();

                LocalLogger.Log("Joining to group");
                JoinToGroup();
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(Start), e);
            }
            HubConnection.Error += exception =>
            {
                if (!(exception is System.TimeoutException))
                {
                    LocalLogger.Log(nameof(HubConnection.Error), exception);
                }
            };
        }

        //private static void CaptureScreen()
        //{
        //    SessionManager.CaptureScreenOfCurrentUser();
        //    var base64Image = Base64Converter.PngFileToBase64(Path.Combine(SessionManager.CurrentUserAppDataFolder, "ss.an"));
        //    SendImage(base64Image);
        //}

        //private static void SendImage(string base64Image)
        //{
        //    if (HubConnection.State != ConnectionState.Connected) return;

        //    try
        //    {
        //        var computer = JsonLocalDatabase.Instance.Computer;
        //        if (computer.ComputerId != 0)
        //        {
        //            _myHubProxy.Invoke("SendImageToPage", computer.ComputerId, base64Image).ContinueWith(task =>
        //            {
        //                if (!task.IsFaulted) return;
        //                if (task.Exception != null)
        //                    LocalLogger.Log("SendImage", task.Exception);
        //            }).Wait();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LocalLogger.Log(nameof(SendImage), e);
        //    }
        //}

        private static void GetInstalledVoicesList(string connectionId)
        {
            try
            {
                LocalLogger.Log("Return installed voices");
                _myHubProxy.Invoke("ShowIntalledVoices", JsonLocalDatabase.Instance.Computer.ComputerId, TextToSpeechHelper.GetInstalledVoicesList())
                    .ContinueWith(
                        task =>
                        {
                            if (!task.IsFaulted) return;
                            if (task.Exception != null)
                                LocalLogger.Log(nameof(GetInstalledVoicesList), task.Exception);
                        });
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(GetInstalledVoicesList), e);
            }
        }

        private static void Speak(string text, string languageCodeOrVoiceName)
        {
            TextToSpeechHelper.Speak(text, languageCodeOrVoiceName);
            LogOnPage($"Voice message received: \"{text}\"");
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
                        if (!task.IsFaulted) return;
                        if (task.Exception != null)
                            LocalLogger.Log("JoinToGroup", task.Exception);
                    }).Wait();
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(JoinToGroup), e);
            }
        }

        public static void LogOnPage(string text)
        {
            Task.Run(() => LogDataOnPage(text));
        }

        private static void LogDataOnPage(string text)
        {
            if (HubConnection.State != ConnectionState.Connected) return;

            try
            {
                var computer = JsonLocalDatabase.Instance.Computer;
                if (computer.ComputerId != 0)
                {
                    _myHubProxy.Invoke("LogDataOnPage", computer.ComputerId, text).ContinueWith(task =>
                    {
                        if (!task.IsFaulted) return;
                        if (task.Exception != null)
                            LocalLogger.Log("LogOnPage", task.Exception);
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
                        if (!task.IsFaulted) return;
                        if (task.Exception != null)
                            LocalLogger.Log("Pong", task.Exception);
                    });
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(HubService), e);
            }
        }

        private static void PowerAction(string nameOfAction, int delayInSeconds)
        {
            var user = SessionManager.GetActive();
            switch (nameOfAction)
            {
                case "lock":
                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        LogOnPage($"Locking computer..");
                        SessionManager.DisconnectCurrentUser();
                        LogOnPage($"Computer locked!");
                    }
                    else
                    {
                        LogOnPage($"No user logged in!");
                    }
                    break;

                case "logoff":
                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        LogOnPage($"Logging off user {user}..");
                        SessionManager.LogOutCurrentUser();
                        LogOnPage($"User {user} has been logged off!");
                    }
                    else
                    {
                        LogOnPage($"No user logged in!");
                    }
                    break;

                case "shutdown":
                    LogOnPage("Shutdown in progress..");
                    PowerAndSessionActions.Shutdown(delayInSeconds);
                    break;

                case "restart":
                    LogOnPage("Restart in progress..");
                    PowerAndSessionActions.Restart();
                    break;

                case "hibernate":
                    LogOnPage("Hibernation in progress..");
                    PowerAndSessionActions.Hibernate();
                    break;

                case "sleep":
                    LogOnPage("Putting computer in sleep mode in progress..");
                    PowerAndSessionActions.Sleep();
                    break;

                default:
                    LocalLogger.Log("Unknown SignalR action has been called. " +
                    $"{nameof(PowerAction)}(string nameOfAction = \"{nameOfAction}\" int delay = {delayInSeconds})");
                    break;
            }
        }

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
    }
}