using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class IntervalTimeLogger
    {
        private static readonly JsonLocalDatabase Db = JsonLocalDatabase.Instance;
        private static Timer _timer;
        public static void Start()
        {
            _timer = new Timer(Run, null, 0, Settings.LogTimeIntervalInSeconds * 1000);
        }

        public static void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();
        }

        private static void Run(object obj)
        {
            var computer = Db.Computer;
            var intervals = computer.LogTimeIntervals?.OrderBy(l => l.StartTime).ToList() ?? new List<LogTimeIntervalLocal>();

            var username = SessionManager.GetActive();
            var interval = intervals.LastOrDefault();
            var currentDateTime = DateTime.Now;
            var state = SessionManager.GetIdleTime().TotalSeconds < Settings.LogTimeIntervalInSeconds * 0.9 ? "Active" : "Idle";
            if (interval == null) interval = new LogTimeIntervalLocal() { StartTime = currentDateTime };
            var timeDifferenceInSeconds = (interval.StartTime + interval.Duration - currentDateTime).TotalSeconds;
            LocalLogger.Log($"TimeDifferenceInSeconds: {timeDifferenceInSeconds}");
            // Usernames and state must be the same.
            // Also time difference between previous and current interval must be less than 70 seconds.
            var isIntervalUpToDate = interval.ComputerUser == username
                                    && interval.State == state
                                    && timeDifferenceInSeconds < Settings.LogTimeIntervalInSeconds * 1.2;
            if (isIntervalUpToDate)
            {
                // Update the current interval
                LocalLogger.Log("Update the current interval");
                interval.Duration += TimeSpan.FromSeconds(Settings.LogTimeIntervalInSeconds);
            }
            else
            {
                // Create a new interval
                LocalLogger.Log("Create a new interval");
                intervals.Add(new LogTimeIntervalLocal()
                {
                    ComputerUser = username,
                    Duration = new TimeSpan(0, 0, Settings.LogTimeIntervalInSeconds),// interval starts with 1 minute
                    StartTime = currentDateTime,
                    State = state,
                });
            }
            computer.LogTimeIntervals = intervals;
            Db.Computer = computer;
        }
    }
}
