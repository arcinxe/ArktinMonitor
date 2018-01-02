using ArktinMonitor.Helpers;
using System;
using System.Linq;

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
                HubService.LogOnPage($"Current user {userName} has limit set to {limit.TimeAmount}, Time used: {totalTime} Time left: {timeLeft}");
                if (timeLeft.Hours == 0 && timeLeft.Minutes == 30)
                {
                    TextToSpeechHelper.Speak($"Because of the time limit set to {limit.TimeAmount.Hours} hours and {limit.TimeAmount.Minutes} minutes, You have {(int)timeLeft.TotalMinutes}  {(timeLeft.Minutes == 1 ? "minute" : "minutes")} left of using the computer today!");
                }
                var reminederMinutes = new[] {20, 10, 5, 2, 1};
                if (timeLeft.Hours == 0 && reminederMinutes.Contains(timeLeft.Minutes))
                {
                    TextToSpeechHelper.Speak($"You have {timeLeft.Minutes} {(timeLeft.Minutes == 1 ? "minute":"minutes")} left!");
                }
                if (!(timeLeft.TotalSeconds < 0)) return;
                LocalLogger.Log($"Logging off user {userName}");
                TextToSpeechHelper.Speak("End of time, logging off!");
                HubService.LogOnPage($"Logging off..");
                SessionManager.LogOutCurrentUser();
                HubService.LogOnPage($"User {userName} has been logged of");
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(TimeLimitsManager), e);
            }
        }
    }
}