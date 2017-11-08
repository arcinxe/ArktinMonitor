using System.Diagnostics;
using System.IO;

namespace ArktinMonitor.IdleTimeCounter
{
    internal class Program
    {
        private static void Main()
        {
           var idleTickCount = LastUserInput.GetIdleTickCount();
           using (var streamWriter = new StreamWriter(Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "IdleTime.an"), false))
            {
                streamWriter.WriteLine(idleTickCount);
            }
        }
    }
}
