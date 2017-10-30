using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class DiskExtension
    {
        public static DiskResourceModel ToResourceModel(this Disk disk)
        {
            return new DiskResourceModel()
            {
              Computer = disk.Computer.ToResourceModel(),
              Name = disk.Name,
              TotalSpace = disk.TotalSpace,
              Letter = disk.Letter,
              FreeSpace = disk.FreeSpace
            };
        }

        public static Disk ToModel(this DiskResourceModel disk)
        {
            return new Disk()
            {
                Name = disk.Name,
                TotalSpace = disk.TotalSpace,
                Letter = disk.Letter,
                FreeSpace = disk.FreeSpace
            };
        }
    }
}
