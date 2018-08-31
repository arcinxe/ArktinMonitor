using ArktinMonitor.Helpers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class HubService
    {
        private static readonly HubConnection HubConnection = new HubConnection(Settings.ApiUrl);

        private static IHubProxy _myHubProxy;
        private static string _connectionId = string.Empty;

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
                if (!HubConnection.Headers.TryGetValue("Authorization", out var value))
                {
                    HubConnection.Headers.Add("Authorization", "Bearer " + bearerToken);
                }
                if (value != "Bearer " + bearerToken)
                {
                    HubConnection.Headers["Authorization"] = "Bearer " + bearerToken;
                }

                if (_myHubProxy == null)
                {
                    _myHubProxy = HubConnection.CreateHubProxy("MyComputerHub");
                    HubConnection.StateChanged += state =>
                    {
                        _connectionId = HubConnection.ConnectionId;
                        if (state.NewState == ConnectionState.Connected) Pong(_connectionId);
                        LocalLogger.Log(
                            $"State of connection to hub changed from {state.OldState} to {state.NewState}");
                    };


                    _myHubProxy.On<string, string>("command", ExecuteCommand);
                    _myHubProxy.On<string, int>("powerAction", PowerAction);
                    _myHubProxy.On<string>("ping", Pong);
                    _myHubProxy.On("requestProcesses", GetProcesses);
                }
                LocalLogger.Log("Starting hub connection");

                HubConnection.Start().Wait();

                LocalLogger.Log("Joining to group");
                JoinToGroup();
                //Pong(_connectionId);
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

        private static void ExecuteCommand(string command, string attributes)
        {
            //TextToSpeechHelper.VoiceDebug($"received command {command}.");// TODO: Remove this
            //TextToSpeechHelper.VoiceDebug($"with following attributes: {attributes}.");// This too
            switch (command)
            {
                case "speak":
                    Speak(attributes);
                    break;
                case "run":
                    RunApp(attributes);
                    break;
                case "keys":
                    SendKeys(attributes);
                    break;
                case "kill":
                    KillProcesses(attributes);
                    break;
                case "text":
                    SendTextMessage(attributes);
                    break;
                case "volume":
                    ManageVolume(attributes);
                    break;
                case "voices":
                    GetInstalledVoicesList();
                    break;
                case "log":
                    LocalLogger.Log("Web browser client: " + attributes);
                    break;
                case "process":
                    GetProcessDetails(attributes);
                    break;
                case "processes":
                    GetProcesses();
                    break;
                case "priority":
                    SetProcessPriority(attributes);
                    break;
                default:
                    //var message = $"Unsupported command [{command}] with attributes {attributes}";
                    //LocalLogger.Log(message);
                    //LogDataOnPage(message);
                    break;
            }
            //var text = $"Command [{command}] with attributes [{attributes}] has been executed";
            //LocalLogger.Log(text);
            //LogDataOnPage(text);
        }

        private static void SetProcessPriority(string attributes)
        {
            //throw new NotImplementedException();
            // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processpriorityclass?redirectedfrom=MSDN&view=netframework-4.7.2
            // var process = Process.GetProcessesByName("notepad");
            //process.FirstOrDefault().PriorityClass = ProcessPriorityClass.RealTime;
        }

        private static void RunApp(string executablePath)
        {
            SessionManager.RunApp(executablePath);
            var message = $"Following app has been executed: \"{executablePath}\"";
            LogOnPage(message);
            LocalLogger.Log(message);
        }

        private static void SendTextMessage(string text)
        {
            SessionManager.SendMessageToCurrentUser(text);
            var message = $"Received text message: \"{text}\"";
            LogOnPage(message);
            LocalLogger.Log(message);
        }

        private static void SendKeys(string keys)
        {
            SessionManager.SendKeys(keys);
            var message = $"Received following keys: [{keys}]";
            LogOnPage(message);
            LocalLogger.Log(message);
        }

        private static void KillProcesses(string nameOrPid)
        {
            LocalLogger.Log($"Killing process: {nameOrPid}");
            var count = ProcessManager.KillThis(nameOrPid);
            var message = count > 0 ? $"Killed {count} process/es" : "No processed killed";
            LogOnPage(message);
            LocalLogger.Log(message);
        }

        private static void GetProcessDetails(string processId)
        {
            if (int.TryParse(processId, out var result))
            {
                var details = Processes.GetProcessDetails(result);
                LocalLogger.Log($"Returning details of process: [{processId}]");
                _myHubProxy.Invoke("FillProcessDetails", JsonLocalDatabase.Instance.Computer.ComputerId, details)
                    .ContinueWith(
                        task =>
                        {
                            if (!task.IsFaulted) return;
                            if (task.Exception != null)
                                LocalLogger.Log(nameof(GetProcessDetails), task.Exception);
                        });
            }
        }

        private static void ManageVolume(string volume)
        {
            LogOnPage($"Current volume: {VolumeChanger.Volume}%");
            if (int.TryParse(volume, out int result))
            {
                VolumeChanger.Volume = result;
                LogOnPage($"Volume changed to {result}%");
            }
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

        private static void GetInstalledVoicesList()
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

        private static void Speak(string text)
        {
            LogOnPage($"Voice message received: \"{text.Split('|').FirstOrDefault()}\"");
            TextToSpeechHelper.Speak(text.Split('|').FirstOrDefault(), text.Split('|').ElementAtOrDefault(1));
        }

        private static void GetProcesses()
        {
            try
            {
                LocalLogger.Log("Returning running processes");
                _myHubProxy.Invoke("SendProcessesToPage", JsonLocalDatabase.Instance.Computer.ComputerId, Processes.GetProcesses()
                        .OrderByDescending(p => p.Session).ThenBy(p => p.Name))
                    .ContinueWith(
                        task =>
                        {
                            if (!task.IsFaulted) return;
                            if (task.Exception != null)
                                LocalLogger.Log(nameof(GetProcesses), task.Exception);
                        });
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(GetProcesses), e);
            }
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