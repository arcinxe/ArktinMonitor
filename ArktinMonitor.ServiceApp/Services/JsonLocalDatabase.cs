using System;
using System.IO;
using ArktinMonitor.Data.Models;
using Newtonsoft.Json;

namespace ArktinMonitor.ServiceApp.Services
{
    internal sealed class JsonLocalDatabase
    {
        private static volatile JsonLocalDatabase _instance;
        private static readonly object SyncRoot = new object();

        public ComputerLocal Computer
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<ComputerLocal>(File.ReadAllText(Path.Combine(Settings.LocalPath, "database.json")))?? new ComputerLocal();
                }
                catch (Exception)
                {
                    if (File.Exists(Path.Combine(Settings.LocalPath, "database.json")))
                    {
                        File.Copy(Path.Combine(Settings.LocalPath, "database.json"), Path.Combine(Settings.LocalPath, $"database {DateTime.Now:yyyy-MM-dd-HH-mm-ss-ff}.err"));
                    }
                    LocalLogger.Log("[Service] - Database not found, returning empty one.");
                    return new ComputerLocal();
                }
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                var jsonFile = JsonConvert.SerializeObject(value, Formatting.Indented);
                using (var streamWriter = new StreamWriter(Path.Combine(Settings.LocalPath, "database.json")))
                {
                    streamWriter.WriteLine(jsonFile);
                }

            }

        }

        private JsonLocalDatabase() { }

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
