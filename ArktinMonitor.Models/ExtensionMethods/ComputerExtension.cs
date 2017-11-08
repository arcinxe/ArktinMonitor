using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

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
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress,
                Disks = disks,
                ComputerUsers = computerUsers,
                LogTimeIntervals = logTimeIntervals
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
            if (oldComputer == null || newComputer == null)
            {
                return true;
            }
            var isDifferent = !(oldComputer.Name == newComputer.Name
                              && oldComputer.Cpu == newComputer.Gpu
                              && Math.Abs(oldComputer.Ram - newComputer.Ram) < 0.10
                              && oldComputer.OperatingSystem == newComputer.OperatingSystem
                              && oldComputer.MacAddress == newComputer.MacAddress);
                
            return isDifferent;
        }
    }
}
