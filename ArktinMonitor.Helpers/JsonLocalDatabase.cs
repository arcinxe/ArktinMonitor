using ArktinMonitor.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArktinMonitor.Helpers
{
    public sealed class JsonLocalDatabase
    {
        private static volatile JsonLocalDatabase _instance;
        private static readonly object SyncRoot = new object();

        private static readonly string DataStoragePath =Settings.ProgramDataPath;

        public ComputerLocal Computer
        {
            get
            {
                try
                {
                    var computer = JsonConvert.DeserializeObject<ComputerLocal>(
                        File.ReadAllText(Path.Combine(DataStoragePath, "database.json"))
                        ) ?? new ComputerLocal();
                    var intervals = JsonConvert.DeserializeObject<List<LogTimeIntervalLocal>>(
                        File.ReadAllText(Path.Combine(DataStoragePath, "LogTimeIntervals.json"))
                                                   ) ?? new List<LogTimeIntervalLocal>();
                    computer.LogTimeIntervals = intervals;
                    return computer;
                }
                catch (Exception)
                {
                    if (File.Exists(Path.Combine(DataStoragePath, "database.json")))
                    {
                        File.Copy(
                            Path.Combine(DataStoragePath, "database.json"),
                            Path.Combine(DataStoragePath, $"database {DateTime.Now:yyyy-MM-dd-HH-mm-ss-ff}.err.json")
                            );
                    }
                    if (File.Exists(Path.Combine(DataStoragePath, "LogTimeIntervals.json")))
                    {
                        File.Copy(
                            Path.Combine(DataStoragePath, "LogTimeIntervals.json"),
                            Path.Combine(DataStoragePath, $"LogTimeIntervals {DateTime.Now:yyyy-MM-dd-HH-mm-ss-ff}.err.json")
                        );
                    }
                    LocalLogger.Log("[Service] - Database not found, returning empty one.");
                    return new ComputerLocal();
                }
            }
            set
            {
                try
                {
                    if (value == null) throw new ArgumentNullException(nameof(value));
                    var logTimeIntervalsFile = JsonConvert.SerializeObject(value.LogTimeIntervals, Formatting.Indented);
                    using (var streamWriter = new StreamWriter(Path.Combine(DataStoragePath, "LogTimeIntervals.json")))
                    {
                        streamWriter.WriteLine(logTimeIntervalsFile);
                    }
                    value.LogTimeIntervals = null;
                    var jsonFile = JsonConvert.SerializeObject(value, Formatting.Indented);
                    using (var streamWriter = new StreamWriter(Path.Combine(DataStoragePath, "database.json")))
                    {
                        streamWriter.WriteLine(jsonFile);
                    }
                }
                catch (Exception e)
                {
                    LocalLogger.Log(nameof(JsonLocalDatabase), e);
                }
            }
        }

        private JsonLocalDatabase()
        {
        }

        public static JsonLocalDatabase Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new JsonLocalDatabase();
                    }
                }
                return _instance;
            }
        }
    }
}