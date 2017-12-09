using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class TempSignalR
    {

        public static void Start()
        {
            var hubConnection = new HubConnection("https://arktin.azurewebsites.net/");
            var token = "HdR3ISBq8HLaczEbn_gXcJfn_MhlNoXZ6ydUoJiJbfx1-i5vtGvBHFlXT0kiACLPFQRj8O_O1XSeQwh-4OmypHKg9FUYrt3frhYXu1k956IODyRNW4hfa7K-e33YcTJtg0begPvEWeS92I0OcdbU-5-8RU0wTira_tUsJhis-o1It7gOv6lJHx_7ssjq7pzM2OaKOlA-3ir9oN40C6V5WrQEsjxEeiuSBaVaGakejvDCwOnivUUr0NQ40ABuDP-KXeElnRfYUxx_xu3OUgO3uj7vmqc2moRnrI7oDQo7mCHlWNwI4icQ4rroNdXBJaJVyayaSTZj7TktnAFCIJmeRGdBv96p0CQlqQf7iquT4KqfT2GrgnUnUhL5othFhe7CUBIKmbWwIGu-rFU-7lE70y6HciaSqxBnJqdvu_jpj8H85_QfkIYUNNzIXoTOSadkXBhKLOtOBauaJ8F5yCvaZwea4uj14hW_feDw-BKSQ7kjMsQDjhWkUCkY6_PR7vpM";
            var token2 = "bC6VvCUPaa2nFXkuOWWX_6ZzGrmDeTdLvJvNkndvFWhdn_SbQn7QOHLx3X4fFCMdJJi8AHr4KKqN3DcBd5OHvmKrKjgquttssV4a3Fu1B4GpOPfddUVak9EBEUYWRnDpyfrHx4-EZriUX57sJnSBuLhNnvA1UISyMOwE1mX7-9OrB30ZjKzIl2Aau6dMjcoEQ8ElSQu2w1innRQfJPSPdeFnYeG6b0yQne5LVHfNjKNJCxeKTxTjaVB2fJEpnHr8otDDKsPeHtmgKs9ZV2qaLAoY-m4rzqzoZTRIiBVHFax_FPgk80S_-3qlbwJ6FuXAGAq9FxHQ7c_CL8bEsop8lBkWVg4M6dLDCCxOz_Rcqvlvb9uyg-AUeBmuR26LedYVVZ-BHCe85XWaoG8dASyc3ltphrLmi4SYQKAAp3aC2aO2jHZODf2rlHX0uvD3165CJGjVT3PxexLjTlg5LFryNjofK_j6O1XGs5QFrkXUWv0";

            //var cookie = new Cookie("ArktinMonitorCookie", token2){Domain = "arktin.azurewebsites.net" };
            //hubConnection.CookieContainer = new CookieContainer();
            //hubConnection.CookieContainer.Add(cookie);
            hubConnection.Headers.Add("Authorization", "Bearer " + token2);
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            IHubProxy myHubProxy = hubConnection.CreateHubProxy("TempHub");

            myHubProxy.On<string, string>("addNewMessageToPage",
                (name, message) => Console.Write($"{name} - {message}\n"));

            hubConnection.Start().Wait();
            //System.Threading.Thread.Sleep(5000);

            while (true)
            {
                //System.Threading.Thread.Sleep(1000);

                myHubProxy.Invoke("Send", "service", Console.ReadLine()).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        if (task.Exception != null)
                            Console.WriteLine("There was an error opening the connection:{0}",
                                task.Exception.GetBaseException());
                    }
                }).Wait();

            }
        }
    }
}
