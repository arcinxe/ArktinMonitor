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
              TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
              Letter = disk.Letter,
              FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes
            };
        }

        public static Disk ToModel(this DiskResourceModel disk)
        {
            return new Disk()
            {
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes
            };
        }


        public static bool NeedsUpdate(this DiskLocal oldDisk, DiskLocal newDisk)
        {
            var isDifferent = !(oldDisk.Name == newDisk.Name
                              && oldDisk.Letter == newDisk.Letter
                              && Math.Abs(oldDisk.FreeSpaceInGigaBytes - newDisk.FreeSpaceInGigaBytes) < 1024 * 1024 * 50 // Tolerance up to 50 MB.
                              && Math.Abs(oldDisk.TotalSpaceInGigaBytes - newDisk.TotalSpaceInGigaBytes) < 1024 * 1024 * 50);
            return isDifferent;
        }
    }
}
