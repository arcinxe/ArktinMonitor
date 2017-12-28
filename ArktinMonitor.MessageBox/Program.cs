using System;
using System.Linq;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.MessageBox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //System.Windows.Forms.MessageBox.Show(args.FirstOrDefault());
            JsonHelper.SerializeToJsonFile(@"d:\test.json", 
                new Data.Models.DailyTimeLimitLocal()
                {
                    Active = true, TimeAmount = new TimeSpan(3,0,0)
                });
        }
    }
}