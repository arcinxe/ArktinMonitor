using System.IO;
using Newtonsoft.Json;

namespace ArktinMonitor.ServiceApp.Helpers
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
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}