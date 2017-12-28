using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class TimeLimitsManager
    {
        public static void Run()
        {
            LocalLogger.Log($"{nameof(TimeLimitsManager)} is running");

            try
            {
                var computer = JsonLocalDatabase.Instance.Computer;

                var userName = SessionManager.GetActive();
                if (string.IsNullOrWhiteSpace(userName))
                {
                    LocalLogger.Log($"User {userName} not found");
                    return;
                }

                var curerentUser = computer.ComputerUsers.FirstOrDefault(u => u.Name == userName);
                var limit = curerentUser?.DailyTimeLimits?.FirstOrDefault(l => l.Active);
                if (limit == null)
                {
                    LocalLogger.Log($"Time limit for user {userName} not found");
                    return;
                }

                var totalTime = new TimeSpan();
                var today = DateTime.Today;
                var logTimeIntervals = computer.LogTimeIntervals
                    .Where(l => l.ComputerUser == userName && l.StartTime.Date == today);

                totalTime = logTimeIntervals.Aggregate(totalTime, (duration, log) => duration + log.Duration);

                var timeLeft = limit.TimeAmount - totalTime;
                LocalLogger.Log($"User {userName} has limit set to {limit.TimeAmount}, Time used: {totalTime} Time left: {timeLeft}");
                if (timeLeft.TotalSeconds < 0)
                {
                    LocalLogger.Log($"Logging off user {userName}");
                    SessionManager.DisconnectCurrentUser();
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(TimeLimitsManager),e);
            }
        }
    }
}
