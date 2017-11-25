using ArktinMonitor.Data.Models;
using System;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class DiskExtension
    {
        public static DiskResourceModel ToResourceModel(this Disk disk)
        {
            return new DiskResourceModel()
            {
                ComputerId = disk.ComputerId,
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes,
                DiskId = disk.DiskId
            };
        }

        public static DiskResourceModel ToResourceModel(this DiskLocal disk, int computerId)
        {
            return new DiskResourceModel()
            {
                ComputerId = computerId,
                Name = disk.Name,
                TotalSpaceInGigaBytes = disk.TotalSpaceInGigaBytes,
                Letter = disk.Letter,
                FreeSpaceInGigaBytes = disk.FreeSpaceInGigaBytes,
                DiskId = disk.DiskId
            };
        }

        public static DiskLocal ToLocal(this DiskResourceModel disk)
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

        public static Disk ToModel(this DiskResourceModel disk)
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