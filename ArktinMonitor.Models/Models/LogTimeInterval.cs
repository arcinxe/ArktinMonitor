using System;

namespace ArktinMonitor.Data.Models
{
    public class LogTimeInterval : BasicLogTimeInterval
    {
        public int LogTimeIntervalId { get; set; }

        public int? ComputerUserId { get; set; }

        public virtual ComputerUser ComputerUser { get; set; }

        public int ComputerId { get; set; }

        public virtual Computer Computer { get; set; }
    }

    public class LogTimeIntervalLocal : BasicLogTimeInterval
    {
        public int LogTimeIntervalId { get; set; }

        public string ComputerUser { get; set; }
    }

    public abstract class BasicLogTimeInterval
    {
        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string State { get; set; }
    }
}
