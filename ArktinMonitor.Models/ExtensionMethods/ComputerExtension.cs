﻿using ArktinMonitor.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class ComputerExtension
    {
        public static ComputerResourceModel ToResourceModel(this Computer computer)
        {
            return new ComputerResourceModel()
            {
                ComputerId = computer.ComputerId,
                Name = computer.Name,
                Cpu = computer.Cpu,
                Gpu = computer.Gpu,
                Ram = computer.Ram,
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress
            };
        }

        public static ComputerViewModel ToViewModel(this Computer computer,
            List<Disk> disks,
            List<ComputerUser> computerUsers,
            List<LogTimeInterval> logTimeIntervals)
        {
            return new ComputerViewModel()
            {
                Name = computer.Name,
                Cpu = computer.Cpu,
                Gpu = computer.Gpu,
                Ram = computer.Ram,
                ComputerId = computer.ComputerId,
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress,
                Disks = disks,
                ComputerUsers = computerUsers,
                LogTimeIntervals = logTimeIntervals.Select(l => l.ToViewModel()).ToList()
            };
        }

        public static ComputerResourceModel ToResourceModel(this ComputerViewModel computer)
        {
            return new ComputerResourceModel()
            {
                Name = computer.Name,
                Cpu = computer.Cpu,
                Gpu = computer.Gpu,
                Ram = computer.Ram,
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress
            };
        }

        public static ComputerResourceModel ToResourceModel(this ComputerLocal computer)
        {
            return new ComputerResourceModel()
            {
                Name = computer.Name,
                Cpu = computer.Cpu,
                Gpu = computer.Gpu,
                Ram = computer.Ram,
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress,
                ComputerId = computer.ComputerId
            };
        }

        public static Computer ToModel(this ComputerResourceModel computer)
        {
            return new Computer()
            {
                ComputerId = computer.ComputerId,
                Name = computer.Name,
                Cpu = computer.Cpu,
                Gpu = computer.Gpu,
                Ram = computer.Ram,
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress
            };
        }

        public static bool NeedsUpdate(this ComputerLocal oldComputer, ComputerLocal newComputer)
        {
            if (oldComputer == null)return true;
            if (newComputer == null)return false;

            var isDifferent = !(oldComputer.Name == newComputer.Name
                              && oldComputer.Cpu == newComputer.Cpu
                              && oldComputer.Gpu == newComputer.Gpu
                              && Math.Abs(oldComputer.Ram - newComputer.Ram) < 0.10
                              && oldComputer.OperatingSystem == newComputer.OperatingSystem
                              && oldComputer.MacAddress == newComputer.MacAddress);

            return isDifferent;
        }
    }
}