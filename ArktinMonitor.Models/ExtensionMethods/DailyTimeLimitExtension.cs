using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class DailyTimeLimitExtension
    {
        public static DailyTimeLimitResource ToResource(this DailyTimeLimit timeLimit)
        {
            return new DailyTimeLimitResource()
            {
                DailyTimeLimitId = timeLimit.DailyTimeLimitId,
                ComputerUserId = timeLimit.ComputerUserId,
                Active = timeLimit.Active,
                TimeAmount = timeLimit.TimeAmount
            };
        }

        public static DailyTimeLimitLocal ToLocal(this DailyTimeLimitResource timeLimit)
        {
            return new DailyTimeLimitLocal()
            {
                Active = timeLimit.Active,
                TimeAmount =timeLimit.TimeAmount,
                DailyTimeLimitId = timeLimit.DailyTimeLimitId
            };
        }
    }
}
