using ArktinMonitor.Data.Models;
using System;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class DiskExtension
    {
        public static DiskResource ToResourceModel(this Disk disk)
        {
            return new DiskResource()
            {
                ComputerId = disk.ComputerId,
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes,
                DiskId = disk.DiskId
            };
        }

        public static DiskResource ToResourceModel(this DiskLocal disk, int computerId)
        {
            return new DiskResource()
            {
                ComputerId = computerId,
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes,
                DiskId = disk.DiskId
            };
        }

        public static DiskLocal ToLocal(this DiskResource disk)
        {
            return new DiskLocal()
            {
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes,
                DiskId = disk.DiskId,
                Synced = true
            };
        }

        public static Disk ToModel(this DiskResource disk)
        {
            return new Disk()
            {
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes,
                ComputerId = disk.ComputerId,
                DiskId = disk.DiskId
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