using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.DataGenerator
{
    internal static class GeneratorLocal
    {
        private static readonly Random Rand = new Random();
        private static readonly GeneratorData Samples = JsonHelper.DeserializeJson<GeneratorData>(Path.Combine(Settings.LocalStoragePath, "samples.json"));


        public static ComputerLocal GenerateComputer()
        {
            var computer = new ComputerLocal()
            {
                Name = $"{Samples.Manufacturers.Random()} - PC",
                Cpu = Samples.Cpus.Random(),
                Gpu = Samples.Gpus.Random(),
                Ram = Math.Pow(2, 6.Random()),
                OperatingSystem = $"Microsoft Windows {Samples.OperatingSystems.Random()}",
                ComputerUsers = new List<ComputerUserLocal>(),
                LogTimeIntervals = new List<LogTimeIntervalLocal>(),
                Disks = new List<DiskLocal>()
            };

            GenerateDisks(computer);
            GenerateComputerUsers(computer);
            GenerateLogTimeIntervals(computer);
            return computer;
        }

        private static void GenerateDisks(ComputerLocal computer)
        {
            var disksAmount = Settings.MaxAmountOfDisks.Random(1);
            var diskLetters = GenerateUniqueLetters(disksAmount).ToArray();
            for (var i = 0; i < disksAmount; i++)
            {
                var totalSpace = 2048.0.Random(10.0);
                var disk = new DiskLocal()
                {
                    TotalSpaceInGigaBytes = totalSpace,
                    FreeSpaceInGigaBytes = totalSpace.Random(0.5),
                    Letter = $"{char.ToUpper(diskLetters.ElementAt(i))}:\\",
                    Name = Samples.DiskNames.Random()
                };
                computer.Disks.Add(disk);
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

        private static void GenerateComputerUsers(ComputerLocal computer)
        {
            var userAmount = Settings.MaxAmountOfComputerUsers.Random(5);
            var names = GenerateUniqueUserNames(userAmount);
            for (var i = 0; i < userAmount; i++)
            {
                var computerUser = new ComputerUserLocal()
                {
                    Name = names.ElementAtOrDefault(i),
                    FullName = 2.Random() == 0 ? names.ElementAtOrDefault(i) : "",
                    PrivilegeLevel = i == 0 ? "Administrator" : (3.Random() == 0 ? "Administrator" : "Standard user"),
                    BlockedApplications = new List<BlockedApplicationLocal>(),
                    BlockedSites = new List<BlockedSiteLocal>()
                };
                GenerateBlockedApps(computerUser);
                GenerateBlockedSites(computerUser);
                computer.ComputerUsers.Add(computerUser);
            }
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
        private static void GenerateLogTimeIntervals(ComputerLocal computer)
        {
            var now = DateTime.Now;
            computer.ComputerUsers.Add(null);
            var dateTime = new DateTime(now.Year, now.Month, now.Day, 2.Random(), 60.Random(), 0);
            for (var i = 0; i < Settings.MaxAmountOfLogTimeIntervals.Random(6); i++)
            {
                var timeSpan = new TimeSpan(2.Random(), 57.Random(), 0);
                var log = new LogTimeIntervalLocal()
                {
                    ComputerUser = computer.ComputerUsers.Random().Name,
                    StartTime = dateTime,
                    Duration = timeSpan,
                    State = computer.ComputerUsers.Random() == null ? "Idle" : (2.Random() == 0 ? "Idle" : "Active"),
                };
                computer.LogTimeIntervals.Add(log);
                computer.ComputerUsers.RemoveAll(u => u == null);
            }
        }

        private static void GenerateBlockedApps(ComputerUserLocal user)
        {
            var appsAmount = Settings.MaxAmountOfBlockedApps.Random();
            for (var i = 0; i < appsAmount; i++)
            {
                var path = Samples.BlockedAppsPaths.Random();
                var app = new BlockedApplicationLocal()
                {
                    Path = path,
                    Name = Path.GetFileNameWithoutExtension(path)
                };
                user.BlockedApplications.Add(app);
            }
        }

        private static void GenerateBlockedSites(ComputerUserLocal user)
        {
            var sitesAmount = Settings.MaxAmountOfBlockedSites.Random();
            for (var i = 0; i < sitesAmount; i++)
            {
                var url = Samples.BlockedSites.Random();
                var site = new BlockedSiteLocal()
                {
                    Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(url.ToLower()),
                    UrlAddress = url
                };
               user.BlockedSites.Add(site);
            }
        }

        /// <summary>
        /// Sends all the data from this database straight to hell where it belongs.
        /// </summary>
        public static void PurgeAll()
        {

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

        ///// <summary>
        ///// Cleans table in database using Entity Framework.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dbSet"></param>
        //public static void Clear<T>(this DbSet<T> dbSet) where T : class
        //{
        //    dbSet.RemoveRange(dbSet);
        //}

        #endregion
    }
}
