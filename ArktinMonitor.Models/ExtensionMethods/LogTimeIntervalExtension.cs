using ArktinMonitor.Data.Models;
using System;
using System.Collections.Generic;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class LogTimeIntervalExtension
    {
        public static LogTimeIntervalResource ToResource(this LogTimeIntervalLocal log, int computerId, string userName)
        {
            return new LogTimeIntervalResource()
            {
                ComputerId = computerId,
                UserName = userName,
                StartTime = log.StartTime,
                Duration = log.Duration,
                State = log.State,
                LogTimeIntervalId = log.LogTimeIntervalId
            };
        }

        public static LogTimeInterval ToModel(this LogTimeIntervalResource log)
        {
            return new LogTimeInterval()
            {
                ComputerId = log.ComputerId,
                ComputerUser = log.UserName,
                StartTime = log.StartTime,
                Duration = log.Duration,
                State = log.State,
                LogTimeIntervalId = log.LogTimeIntervalId
            };
        }

        public static LogTimeIntervalResource ToResource(this LogTimeInterval log)
        {
            return new LogTimeIntervalResource()
            {
                ComputerId = log.ComputerId,
                UserName = log.ComputerUser,
                StartTime = log.StartTime,
                Duration = log.Duration,
                State = log.State,
                LogTimeIntervalId = log.LogTimeIntervalId
            };
        }

        public static LogTimeIntervalViewModel ToViewModel(this LogTimeInterval log)
        {
            return new LogTimeIntervalViewModel()
            {
                User = log.ComputerUser,
                StartTime = log.StartTime,
                Duration = log.Duration,
                State =log.State
            };
        }
    }
}