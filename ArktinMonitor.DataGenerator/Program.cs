using System;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.DataGenerator
{
    internal class Program
    {
        private static void Main()
        {
            LocalLogger.SaveOnDisk = false;
            while (true)
            {

                Console.Title = "Initializing...";

                try
                {
                    Console.WriteLine("Enter amount of Web Accounts to generate or leave empty to purge all tables");
                    if (int.TryParse(Console.ReadLine(), out int result))
                    {
                        Settings.AmountOfWebAccounts = result;
                        LocalLogger.Log("The data generation has been started!");
                        Generator.GenerateWebAccounts();
                    }
                    else
                    {
                        Generator.PurgeAll();
                    }
                }
                catch (Exception e)
                {
                    LocalLogger.Log("Generator", e);
                }
                Console.ReadLine();
            }
        }
    }
}
