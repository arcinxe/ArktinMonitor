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
                try
                {
                    using (var streamWriter = new StreamWriter(path, false))
                    {
                        streamWriter.WriteLine(jsonFile);
                    }

                }
                catch (Exception e)
                {
                    LocalLogger.Log(nameof(SerializeToJsonFile),e);
                }
            }
        }

        public static T DeserializeJson<T>(string path)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                return result;
            }
            catch (Exception)
            {
                //LocalLogger.Log($"[Deserializer] Path [{path}] not found");
                return default(T);
            }
        }
    }
}