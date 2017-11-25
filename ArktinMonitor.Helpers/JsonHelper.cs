using Newtonsoft.Json;
using System;
using System.IO;

namespace ArktinMonitor.Helpers
{
    public static class JsonHelper
    {
        // Saves object as a json file on disk.
        public static void SerializeToJsonFile(string path, object obj)
        {
            lock (obj)
            {
                var jsonFile = JsonConvert.SerializeObject(obj, Formatting.Indented);
                using (var streamWriter = new StreamWriter(path, false))
                {
                    streamWriter.WriteLine(jsonFile);
                }
            }
        }

        public static T DeserializeJson<T>(string path)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            catch (Exception)
            {
                LocalLogger.Log($"[Deserializer] Path [{path}] not found");
                return default(T);
            }
        }
    }
}