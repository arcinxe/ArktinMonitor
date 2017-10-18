using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.ConsoleClient.Helpers;

namespace ArktinMonitor.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db =new Models.ArktinMonitorDataAccess())
            {
                var email = Console.ReadLine();
                if (!(db.WebAccounts.Any(a => a.Email == email)))
                {
                    var webAccount = new Models.WebAccount()
                    {
                        Name = "Marcin",
                        Email = "marcinxe@gmail.com"
                    };
                    db.WebAccounts.Add(webAccount);
                }
                var computer = ComputerHelper.GetComputer();
                db.Computers.Add(computer);
                db.SaveChanges();
            }
            Console.WriteLine("done");
            Console.Read();
        }
    }
}
