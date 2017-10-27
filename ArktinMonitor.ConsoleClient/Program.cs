using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.ConsoleClient.Helpers;
using ArktinMonitor.ConsoleClient.Services;
using ArktinMonitor.Data;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Data.ResourceModels;
using ServiceStack.Text;

namespace ArktinMonitor.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var db = new Models.ArktinMonitorDataAccess())
            //{
            //    var email = Console.ReadLine();
            //    var name = Console.ReadLine();
            //    if (!(db.WebAccounts.Any(a => a.Email == email)))
            //    {
            //        var webAccount = new Models.WebAccount()
            //        {
            //            Name = name,
            //            Email = email
            //        };
            //        db.WebAccounts.Add(webAccount);
            //        db.SaveChanges();
            //    }
            //    var computer = ComputerHelper.GetComputer();
            //    computer.WebAccountId = db.WebAccounts.FirstOrDefault(wa => wa.Email == email).WebAccountId;
            //    db.Computers.Add(computer);
            //    db.SaveChanges();
            //    foreach (var computerDisk in computer.Disks)
            //    {
            //        computerDisk.ComputerId = computer.ComputerId;
            //        db.Disks.Add(computerDisk);
            //    }
            //    foreach (var computerUser in computer.ComputerUsers)
            //    {
            //        computerUser.ComputerId = computer.ComputerId;
            //        db.ComputerUsers.Add(computerUser);
            //    }
            //    db.SaveChanges();
            //}
            var serverClient = new ServerClient();
            var comp = ComputerHelper.GetComputer().ToResourceModel();

            try
            {
                Authorization.RenewBearerToken("marcinxe@gmail.com", "[REDACTED]");
                var sc = serverClient.SendToServer("api/Computers", comp, Authorization.GetBearerToken.AccessToken);
                var response = sc.Result;
                var content = response.Content.ReadAsAsync<ComputerResourceModel>().Result;
                LocalLogger.Log($"ComputerId: {content.MacAddress}");
                Console.WriteLine(content.Dump());
            }
            catch (Exception e)
            {
                LocalLogger.Log("Local", e);
            }
            //var computer2 = ComputerHelper.GetComputer();
            //JsonHelper.SerializeToJsonFile(Path.Combine(Settings.LocalPath, "computer.json"), computer2);
            //LocalLogger.Log("test");
            //LocalLogger.Log(computer2.Dump());
            Authorization.RenewBearerToken("marcinxe@gmail.com", "[REDACTED]");
            var token = Authorization.GetBearerToken;
            Console.WriteLine(token.Dump());
            Console.WriteLine(token.Expires.ToLongDateString() + " " + token.Expires.ToLongTimeString());

            Console.Read();
        }
    }
}
