using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Data.Models
{
    public class DailyTimeLimit
    {
        public int DailyTimeLimitId { get; set; }

        public TimeSpan TimeAmount { get; set; }

        public bool Active { get; set; }

        public int ComputerUserId { get; set; }

        public virtual ComputerUser ComputerUser { get; set; }
    }
}
