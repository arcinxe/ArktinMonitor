using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.ConsoleClient.Helpers;
using ArktinMonitor.ConsoleClient.Services;
using ServiceStack.Text;

namespace ArktinMonitor.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Models.ArktinMonitorDataAccess())
            {
                var email = Console.ReadLine();
                var name = Console.ReadLine();
                if (!(db.WebAccounts.Any(a => a.Email == email)))
                {
                    var webAccount = new Models.WebAccount()
                    {
                        Name = name,
                        Email = email
                    };
                    db.WebAccounts.Add(webAccount);
                    db.SaveChanges();
                }
                var computer = ComputerHelper.GetComputer();
                computer.WebAccountId = db.WebAccounts.FirstOrDefault(wa => wa.Email == email).WebAccountId;
                db.Computers.Add(computer);
                db.SaveChanges();
                foreach (var computerDisk in computer.Disks)
                {
                    computerDisk.ComputerId = computer.ComputerId;
                    db.Disks.Add(computerDisk);
                }
                foreach (var computerUser in computer.ComputerUsers)
                {
                    computerUser.ComputerId = computer.ComputerId;
                    db.ComputerUsers.Add(computerUser);
                }
                db.SaveChanges();
            }

            var computer2 = ComputerHelper.GetComputer();
            Helpers.JsonHelper.SerializeToJsonFile(Path.Combine(Settings.LocalPath, "computer.json"),computer2);
            LocalLogger.Log("test");
            LocalLogger.Log(computer2.Dump());
            Console.Read();
        }
    }
}
