using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace ArktinMonitor.DataGenerator
{
    internal static class Generator
    {
        private static readonly Random Rand = new Random();
        private static readonly GeneratorData Samples = JsonHelper.DeserializeJson<GeneratorData>(Path.Combine(Settings.LocalStoragePath, "samples.json"));
        private static readonly ArktinMonitorDataAccess Db = new ArktinMonitorDataAccess();
        private static int _computersCounter, _computerUsersCounter, _blockedAppsCounter, _blockedSitesCounter, _disksCounter;

        public static void GenerateWebAccounts()
        {
            var start = DateTime.Now;
            for (var i = 0; i < Settings.AmountOfWebAccounts; i++)
            {
                Console.Title = $"Processing... {(double)i / Settings.AmountOfWebAccounts:P} done.";
                var name = Samples.UserNames.Random();
                var webAccount = new WebAccount
                {
                    Name = name,
                    Email = $"{name.ToLower()}{i}@gmail.com",
                };
                Db.WebAccounts.Add(webAccount);
                Db.SaveChanges();
                LocalLogger.Log("#################################################################");
                LocalLogger.Log("WebAccount created.");
                LocalLogger.Log($"{webAccount.Name} - {webAccount.Email}");
                LocalLogger.Log();
                GenerateComputers(webAccount.WebAccountId);
            }
            Console.Title = "Done!";
            LocalLogger.Log(" ------- SUMMARY -------");
            LocalLogger.Log($" Finished in {DateTime.Now - start:mm\\m\\:ss\\s}");
            LocalLogger.Log($" {Settings.AmountOfWebAccounts} Web accounts");
            LocalLogger.Log($" {_computersCounter} Computers");
            LocalLogger.Log($" {_disksCounter} Disks");
            LocalLogger.Log($" {_computerUsersCounter} Computer users");
            LocalLogger.Log($" {_blockedAppsCounter} Blocked apps");
            LocalLogger.Log($" {_blockedSitesCounter} Blocked sites");
        }

        private static void GenerateComputers(int webAccountId)
        {
            var computersAmount = Settings.MaxAmountOfCOmputers.Random();
            for (var i = 0; i < computersAmount; i++)
            {
                var computer = new Computer
                {
                    Name = $"{Samples.Manufacturers.Random()} - PC",
                    WebAccountId = webAccountId,
                    Cpu = Samples.Cpus.Random(),
                    Gpu = Samples.Gpus.Random(),
                    Ram = Math.Pow(2, 6.Random()),
                    OperatingSystem = $"Microsoft Windows {Samples.OperatingSystems.Random()}"
                };
                Db.Computers.Add(computer);
                Db.SaveChanges();
                LocalLogger.Log($"      Computer {computer.Name} running {computer.OperatingSystem} has been created.");
                LocalLogger.Log($"      {computer.Ram}GB RAM, {computer.Cpu}, {computer.Gpu}");
                LocalLogger.Log();
                GenerateDisks(computer.ComputerId);
                var computerUsersIds = GenerateComputerUsers(computer.ComputerId);
                GenerateLogTimeIntervals(computer.ComputerId, computerUsersIds);
                LocalLogger.Log();
                _computersCounter++;
            }
        }

        private static void GenerateDisks(int computerId)
        {
            var disksAmount = Settings.MaxAmountOfDisks.Random(1);
            var diskLetters = GenerateUniqueLetters(disksAmount).ToArray();
            for (var i = 0; i < disksAmount; i++)
            {
                var totalSpace = 2048.0.Random(10.0);
                var disk = new Disk
                {
                    ComputerId = computerId,
                    TotalSpaceInGigaBytes = totalSpace,
                    FreeSpaceInGigaBytes = totalSpace.Random(0.5),
                    Letter = $"{char.ToUpper(diskLetters.ElementAt(i))}:\\",
                    Name = Samples.DiskNames.Random()
                };
                Db.Disks.Add(disk);
                Db.SaveChanges();
                LocalLogger.Log($"              Disk {disk.Name} ({disk.Letter.Replace("\\", "")}) added.");
                LocalLogger.Log($"              Free space: {disk.FreeSpaceInGigaBytes:##.###} GB of total {disk.TotalSpaceInGigaBytes:##.###} GB");
                LocalLogger.Log();
                _disksCounter++;
            }
        }

        private static IEnumerable<char> GenerateUniqueLetters(int amount)
        {
            if (amount > 26)
            {
                throw new Exception("Looping forever...");
            }
            var letters = new List<char>() { 'c' };
            while (letters.Count < amount)
            {
                var letter = (char)('a' + Rand.Next(0, 26));
                if (letters.All(l => l != letter))
                {
                    letters.Add(letter);
                }
            }
            return letters;
        }

        private static List<int?> GenerateComputerUsers(int computerId)
        {
            var computerUsersIds = new List<int?>();
            var userAmount = Settings.MaxAmountOfComputerUsers.Random(1);
            var names = GenerateUniqueUserNames(userAmount);
            for (var i = 0; i < userAmount; i++)
            {
                var computerUser = new ComputerUser
                {
                    ComputerId = computerId,
                    Name = names.ElementAtOrDefault(i),
                    FullName = 2.Random() == 0 ? names.ElementAtOrDefault(i) : "",
                    PrivilegeLevel = i == 0 ? "Administrator" : (3.Random() == 0 ? "Administrator" : "Standard user")
                };
                Db.ComputerUsers.Add(computerUser);
                Db.SaveChanges();
                LocalLogger.Log($"              Computer user {computerUser.Name} of type {computerUser.PrivilegeLevel} added");
                //LocalLogger.Log($"              Account type {computerUser.PrivilegeLevel}");
                GenerateBlockedApps(computerUser.ComputerUserId);
                GenerateBlockedSites(computerUser.ComputerUserId);
                LocalLogger.Log();
                _computerUsersCounter++;
                computerUsersIds.Add(computerUser.ComputerUserId);
            }
            return computerUsersIds;
        }

        private static List<string> GenerateUniqueUserNames(int amount)
        {
            if (amount > Samples.UserNames.Count)
            {
                throw new Exception("Looping forever...");
            }
            var names = new List<string>();
            while (names.Count < amount)
            {
                var name = Samples.UserNames.Random();
                if (names.All(n => n != name))
                {
                    names.Add(name);
                }
            }
            return names;
        }

        private static void GenerateLogTimeIntervals(int computerId, List<int?> usersIds)
        {
            var now = DateTime.Now;
            var dateTime = new DateTime(now.Year, now.Month, now.Day, 2.Random(), 60.Random(), 0);
            usersIds.Add(null);
            for (var i = 0; i < Settings.MaxAmountOfLogTimeIntervals.Random(2); i++)
            {
                var timeSpan = new TimeSpan(2.Random(), 57.Random(), 0);
                var userId = usersIds.Random();
                var log = new LogTimeInterval
                {
                    LogTimeIntervalId = 0,
                    ComputerId = computerId,
                    StartTime = dateTime,
                    Duration = timeSpan,
                    ComputerUserId = userId,
                    State = userId == null ? "Idle" : (2.Random() == 0 ? "Idle" : "Active")
                };
                Db.LogTimeIntervals.Add(log);
                Db.SaveChanges();
                var name = Db.ComputerUsers.FirstOrDefault(cu => cu.ComputerUserId == log.ComputerUserId.Value)?.Name ?? "";
                LocalLogger.Log($"              LogTimeInterval Starting at {log.StartTime:g} and lasting {log.Duration:g} added");
                LocalLogger.Log($"              User name: {name}, state: {log.State}");
                dateTime += timeSpan;
            }
        }

        private static void GenerateBlockedApps(int computerUserId)
        {
            var appsAmount = Settings.MaxAmountOfBlockedApps.Random();
            for (var i = 0; i < appsAmount; i++)
            {
                var path = Samples.BlockedAppsPaths.Random();
                var app = new BlockedApplication
                {
                    ComputerUserId = computerUserId,
                    Path = path,
                    Name = Path.GetFileNameWithoutExtension(path)
                };
                Db.BlockedApplications.Add(app);
                Db.SaveChanges();
                LocalLogger.Log($"                      App {app.Name} has been blocked.");
                //LocalLogger.Log($"                      FilePath {app.FilePath}");
                //LocalLogger.Log();
                _blockedAppsCounter++;
            }
        }

        private static void GenerateBlockedSites(int computerUserId)
        {
            var sitesAmount = Settings.MaxAmountOfBlockedSites.Random();
            for (var i = 0; i < sitesAmount; i++)
            {
                var url = Samples.BlockedSites.Random();
                var site = new BlockedSite
                {
                    ComputerUserId = computerUserId,
                    Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(url.ToLower()),
                    UrlAddress = url
                };
                Db.BlicBlockedSites.Add(site);
                Db.SaveChanges();
                LocalLogger.Log($"                      Site {site.UrlAddress} has been blocked");
                //LocalLogger.Log();
                _blockedSitesCounter++;
            }
        }

        /// <summary>
        /// Sends all the data from this database straight to hell where it belongs.
        /// </summary>
        public static void PurgeAll()
        {
            LocalLogger.Log("The purge has been started!");
            var startTime = DateTime.Now;
            LocalLogger.Log("Purging BlockedSites...");
            Db.BlicBlockedSites.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table BlockedSites has been purged!");
            LocalLogger.Log("Purging BlockedApplications...");
            Db.BlockedApplications.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table BlockedApplications has been purged!");
            LocalLogger.Log("Purging LogTimeIntervals...");
            Db.LogTimeIntervals.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table LogTimeIntervals has been purged!");
            LocalLogger.Log("Purging ComputerUsers...");
            Db.ComputerUsers.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table ComputerUsers has been purged!");
            LocalLogger.Log("Purging Disks...");
            Db.Disks.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table Disks has been purged!");
            LocalLogger.Log("Purging Computers...");
            Db.Computers.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table Computers has been purged!");
            LocalLogger.Log("Purging WebAccounts...");
            Db.WebAccounts.Clear();
            Db.SaveChanges();
            LocalLogger.Log("Table WebAccounts has been purged!");
            LocalLogger.Log("All clean!");
            LocalLogger.Log($"Finished in {DateTime.Now - startTime:mm\\m\\:ss\\s\\:ff\\m\\s}");
        }

        #region Extension methods

        /// <summary>
        /// Returns random element from the collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>Random string fromthe collection</returns>
        private static T Random<T>(this List<T> collection)
        {
            return collection.ElementAtOrDefault(Rand.Next(collection.Count));
        }

        /// <summary>
        /// Returns random integer between min value and this number.
        /// </summary>
        /// <param name="min">lowest returned value</param>
        /// <param name="max"></param>
        /// <returns>Random integer between 0 (or min value) and this number</returns>
        private static int Random(this int max, int min = 0)
        {
            return Rand.Next(min, max + min);
        }

        /// <summary>
        /// Returns random double between min value and this number.
        /// </summary>
        /// <param name="min">lowest returned value</param>
        /// <param name="max"></param>
        /// <returns>Random integer between 0 and this number</returns>
        private static double Random(this double max, double min = 0)
        {
            return Rand.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Cleans table in database using Entity Framework.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        #endregion Extension methods
    }
}