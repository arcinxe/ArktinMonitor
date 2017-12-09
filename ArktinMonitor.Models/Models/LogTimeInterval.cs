using System;
using System.ComponentModel.DataAnnotations;

namespace ArktinMonitor.Data.Models
{
    public class LogTimeInterval : BasicLogTimeInterval
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public new DateTime StartTime { get; set; }

        public int LogTimeIntervalId { get; set; }

        public string ComputerUser { get; set; }

        //public int? ComputerUserId { get; set; }

        //public virtual ComputerUser ComputerUser { get; set; }

        public int ComputerId { get; set; }

        public virtual Computer Computer { get; set; }
    }

    public class LogTimeIntervalViewModel : BasicLogTimeInterval
    {
        public string User { get; set; }

        public new double StartTime { get; set; }

        public new int Duration { get; set; }

    }

    public class LogTimeIntervalResource : BasicLogTimeInterval
    {
        public int LogTimeIntervalId { get; set; }

        public string UserName { get; set; }

        public int ComputerId { get; set; }
    }

    public class LogTimeIntervalLocal : BasicLogTimeInterval
    {
        public int LogTimeIntervalId { get; set; }

        public bool Synced { get; set; }

        public string ComputerUser { get; set; }
    }

    public abstract class BasicLogTimeInterval
    {
        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string State { get; set; }
    }
}