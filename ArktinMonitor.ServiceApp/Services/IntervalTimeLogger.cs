using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class IntervalTimeLogger
    {
        private static readonly JsonLocalDatabase Db = JsonLocalDatabase.Instance;
        //private static Timer _timer;
        //public static void Start()
        //{
        //    _timer = new Timer(Run, null, 0, Settings.LogTimeIntervalInSeconds * 1000);
        //}

        //public static void Stop()
        //{
        //    _timer.Change(Timeout.Infinite, Timeout.Infinite);
        //    _timer.Dispose();
        //}

        //private static void Run(object obj)
        public static void Run()
        {
            var computer = Db.Computer;
            var intervals = computer.LogTimeIntervals?.OrderBy(l => l.StartTime).ToList() ?? new List<LogTimeIntervalLocal>();

            var username = SessionManager.GetActive();
            var interval = intervals.LastOrDefault();
            var currentDateTime = DateTime.Now;
            var state = SessionManager.GetIdleTime().TotalSeconds < 120 ? "Active" : "Idle";
            if (interval == null) interval = new LogTimeIntervalLocal() { StartTime = currentDateTime };
            var timeDifference = (currentDateTime - (interval.StartTime + interval.Duration));
            LocalLogger.Log($"TimeDifferenceInSeconds: {timeDifference}");
            // User names and state must be the same.
            // Also time difference between previous and current interval must be less than 2 minutes.
            var isIntervalUpToDate = interval.ComputerUser == username
                                    && interval.State == state
                                    && timeDifference.TotalSeconds < 120;
            if (isIntervalUpToDate)
            {
                // Update the current interval
                LocalLogger.Log("Update the current interval");
                interval.Duration += timeDifference;
                interval.Synced = false;
            }
            else
            {
                // Create a new interval
                LocalLogger.Log("Create a new interval");
                intervals.Add(new LogTimeIntervalLocal()
                {
                    ComputerUser = username,
                    Duration = new TimeSpan(0, 0, 1),// interval starts with 0 minutes
                    StartTime = currentDateTime,
                    State = state,
                    Synced = false
                });
            }
            computer.LogTimeIntervals = intervals;
            Db.Computer = computer;
        }
    }
}