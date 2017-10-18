using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }
        public string Name { get; set; }
        public string Cpu { get; set; }
        public string Gpu { get; set; }
        public double Ram { get; set; }
        public string OperatingSystem { get; set; }


        public int WebAccountId { get; set; }
        public virtual WebAccount WebAccount { get; set; }
    }
}
