using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Data.ResourceModels;
using ArktinMonitor.Data.ViewModels;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class ComputerExtension
    {
        public static ComputerResourceModel ToResourceModel(this Computer computer)
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
                Name = computer.Name,
                Cpu = computer.Cpu,
                Gpu = computer.Gpu,
                Ram = computer.Ram,
                OperatingSystem = computer.OperatingSystem,
                MacAddress = computer.MacAddress
            };
        }
    }
}
