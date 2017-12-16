﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;
using Microsoft.AspNet.SignalR.Client;
using System.Speech.Synthesis;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class HubService
    {
        private static readonly HubConnection HubConnection = new HubConnection(Settings.ApiUrl);
        private static IHubProxy _myHubProxy;
        public static void Start()
        {

            #region Saved Tokens
            var bearerToken = "7KWzUFpTmXjYPHJf45R9XKkoEGrtanwgiCLzfBoPDOaFivZ0HFpuF0I5j1X0KYfYywCBPbxVjDOdN6UD3UP5fPE4mguU41E3C6HvGHgFS4XQMOOfpcfOSwQEklBLBnIYbs-XcDt2U2o-KaiDYpsAuCVq6va8lqd_QBLiCYrWkbllYScFNnG05zPgsZ6N27mYyEUNhAxnaLXBou7QR9EsxEqHjJLLKYPxcWNHMmjRF8BiufEW8f711Zaf8eX3m7_WKyR6RRF65vI7spEX8JW2-RDs9iI-rjC8Q696mWlp5XJp3R0o3WyVHA47rm0X83dezBtAZj6AkahnYlJVWhWVn_nmSKX-ROdZ32UBVNhYsP5kNJICCIMdVt5ycU2qb_NC3dGt1_yRu0aktHOISw_ZAsvRIhCz6BmP9842VBauDDXdkIdnLb5OZgvFZR0lLeZFE6EdnqI4Q8aNuqfE1VeG_fx44ybODYkXAQbMdeVCYX0"; //"9AmuGmqFhoyXXU3lDNuxrIyml70WvmFSZkLCmQCE47R7h2AU0Z7R91KtXgqr3sJ5U61wkcJF_WFdnJE4VHbTiQxPYcCplQzfTBMZG-jtj_4MGgnndDBIlSC9wEXJLQWhxQeCAoT5EQ9KF4_i9AKbsUMiFscmDEl7PBitaBJeIfSV0GLMnY2_AfTBYbP72iP9n39QJkJEM6mmfCur4Oy1Elv9g3XAPsMXnANkWP6o_d-AwhP5aGtrODp5PbJCwF6iJE5mALbrj6BaK5VAc3YgsLwqXpM_bNCYHJrNBssq8l4QhtbWLBS_UDSgne0swg041wvtLIGjnqJsrtFn-IND4iXmRJ6prXRB1kmg8gI_XJm1ZxxgT9lYT-IJj7SPMMQZAuQebjxFMwduTcSAP8ISIa4CUyoT47uBPAl7nOg_Ee7K2kyOunDUBIvjPK2HsJZsu1YuDgEMURlBob32SNXj0025BhgG6xLWZjddWtTebx5ft61l6ReR4gBO6nBUpYSs";
            #endregion

            var credentialsManager = new CredentialsManager(Settings.ApiUrl, Settings.UserRelatedStoragePath, Settings.SystemRelatedStoragePath, "ArktinMonitor");

            bearerToken = credentialsManager.LoadJsonWebToken().AccessToken;

            HubConnection.Headers.Add("Authorization", "Bearer " + bearerToken);

            _myHubProxy = HubConnection.CreateHubProxy("MyComputerHub");

            _myHubProxy.On<string, string>("addNewMessageToPage",
                (name, message) => Console.Write($"{name} - {message}\n"));

            _myHubProxy.On<string, string>("fart", TextToSpeechHelper.Speak);
            _myHubProxy.On<string>("ping", Pong);
            //myHubProxy.On("fart", () => Console.WriteLine("Purrrrrr!"));

            HubConnection.Start().Wait();

            var computer = JsonLocalDatabase.Instance.Computer;
            if (computer.ComputerId != 0)
            {
                _myHubProxy.Invoke("JoinToGroup", computer.ComputerId).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        if (task.Exception != null)
                            Console.WriteLine("There was an error opening the connection:{0}",
                                task.Exception.GetBaseException());
                    }
                }).Wait();
            }
            //while (true)
            //{
            //    System.Threading.Thread.Sleep(100);

            //    myHubProxy.Invoke("Send", "service", Console.ReadLine()).ContinueWith(task =>
            //    {
            //        if (task.IsFaulted)
            //        {
            //            if (task.Exception != null)
            //                Console.WriteLine("There was an error opening the connection:{0}",
            //                    task.Exception.GetBaseException());
            //        }
            //    }).Wait();

            //}
        }


        private static void Speak()
        {
            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            synth.Speak("This example demonstrates a basic use of Speech Synthesizer");
        }

        private static void Pong(string connectionId)
        {
            try
            {
                Console.WriteLine("Pong");
                _myHubProxy.Invoke("Pong", JsonLocalDatabase.Instance.Computer.ComputerId, connectionId).ContinueWith(
                    task =>
                    {
                        if (task.IsFaulted)
                        {
                            if (task.Exception != null)
                                Console.WriteLine("There was an error opening the connection:{0}",
                                    task.Exception.GetBaseException());
                        }
                    });

            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(HubService), e);
            }
        }
        public static void Stop()
        {
            HubConnection.Stop();
        }

    }
}
