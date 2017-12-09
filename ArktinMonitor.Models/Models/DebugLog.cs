using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Data.Models
{
    public class DebugLog
    {
        public int DebugLogId { get; set; }

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
