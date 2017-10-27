using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Data.ResourceModels
{
    public class ComputerResourceModel
    {
        public string Name { get; set; }

        public string Cpu { get; set; }

        public string Gpu { get; set; }

        public double Ram { get; set; }

        public string OperatingSystem { get; set; }

        public string MacAddress { get; set; }
    }
}
