using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Helpers
{
    public class KeySender
    {

        public static void Test(string data)
        {
            AutoIt.AutoItX.Send(data);
            
        }
    }
}
