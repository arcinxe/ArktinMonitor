﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArktinMonitor.Data.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }
        public string Name { get; set; }
        public string Cpu { get; set; }
        public string Gpu { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        public double Ram { get; set; }
        public string OperatingSystem { get; set; }
        public string MacAddress { get; set; }


        public int WebAccountId { get; set; }
        public virtual WebAccount WebAccount { get; set; }
    }



}