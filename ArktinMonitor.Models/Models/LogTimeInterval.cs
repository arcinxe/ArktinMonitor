using System;

namespace ArktinMonitor.Data.Models
{
    public class LogTimeInterval
    {
        public string LogTimeIntervalId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string State { get; set; }

        public int? ComputerUserId { get; set; }
        public virtual ComputerUser ComputerUser { get; set; }
        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }
}
