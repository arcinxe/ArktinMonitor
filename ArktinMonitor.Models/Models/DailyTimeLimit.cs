using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Data.Models
{
    public class DailyTimeLimit : BasicDailyTimeLimit
    {
        public int DailyTimeLimitId { get; set; }

        public int ComputerUserId { get; set; }

        public virtual ComputerUser ComputerUser { get; set; }
    }

    public class DailyTimeLimitLocal : BasicDailyTimeLimit
    {
        public int DailyTimeLimitId { get; set; }
    }

    public class DailyTimeLimitResource : BasicDailyTimeLimit
    {
        public int DailyTimeLimitId { get; set; }

        public int ComputerUserId { get; set; }

    }

    public abstract class BasicDailyTimeLimit
    {
        public TimeSpan TimeAmount { get; set; }

        public bool Active { get; set; }
    }
}
