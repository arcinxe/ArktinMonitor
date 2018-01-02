using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Helpers;
using System;
using System.Linq;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class DataUpdateManager
    {
        public static void UpdateComputer()
        {
            LocalLogger.Log($"Method {nameof(UpdateComputer)} is running");
            HubService.LogOnPage("Updating computer data");

            var newComputer = ComputerHelper.GetComputer();
            try
            {
                var db = JsonLocalDatabase.Instance;
                var computer = db.Computer;
                var needsUpdate = computer.NeedsUpdate(newComputer);
                if (!needsUpdate) return;
                computer.Name = newComputer.Name;
                computer.Cpu = newComputer.Cpu;
                computer.Gpu = newComputer.Gpu;
                computer.Ram = newComputer.Ram;
                computer.MacAddress = newComputer.MacAddress;
                computer.OperatingSystem = newComputer.OperatingSystem;
                computer.Synced = false;

                db.Computer = computer;
            }
            catch (Exception e)
            {
                LocalLogger.Log("UpdateComputer", e);
            }
        }

        public static void UpdateDisks()
        {
            LocalLogger.Log($"Method {nameof(UpdateDisks)} is running");
            HubService.LogOnPage("Updating disks data");
            var newDisks = ComputerHelper.GetDisks();
            try
            {
                var db = JsonLocalDatabase.Instance;
                var computer = db.Computer;
                if (computer.Disks == null) computer.Disks = newDisks;
                foreach (var newDisk in newDisks)
                {
                    var disk = computer.Disks.FirstOrDefault(d => d.Letter == newDisk.Letter);
                    if (disk == null)
                    {
                        computer.Disks.Add(newDisk);
                    }
                    else
                    {
                        var diskChanged = disk.Name != newDisk.Name
                            || Math.Abs(disk.TotalSpaceInGigaBytes - newDisk.TotalSpaceInGigaBytes) > 0.05
                            || Math.Abs(disk.FreeSpaceInGigaBytes - newDisk.FreeSpaceInGigaBytes) > 0.05; // Tolerance up to 50 MB.

                        if (diskChanged)
                        {
                            disk.Name = newDisk.Name;
                            disk.FreeSpaceInGigaBytes = newDisk.FreeSpaceInGigaBytes;
                            disk.TotalSpaceInGigaBytes = newDisk.TotalSpaceInGigaBytes;
                            disk.Synced = false;
                        }
                    }
                }
                var removedDisks = computer.Disks.Except(newDisks).ToList();
                removedDisks.ForEach(rd => computer.Disks.Remove(rd));
                if (removedDisks.Any() && computer.Disks.FirstOrDefault() != null)
                {
                    computer.Disks.FirstOrDefault().Synced = false;
                }

                db.Computer = computer;
            }
            catch
                (Exception e)
            {
                LocalLogger.Log("UpdateDisks", e);
            }
        }

        public static void UpdateUsers()
        {
            LocalLogger.Log($"Method {nameof(UpdateUsers)} is running");
            HubService.LogOnPage("Updating users");
            try
            {
                var newComputerUsers = ComputerUsersHelper.GetComputerUsers();
                var db = JsonLocalDatabase.Instance;
                var computer = db.Computer;
                var equal = db.Computer.ComputerUsers != null && newComputerUsers.All(computer.ComputerUsers.Contains);
                if (equal && db.Computer.ComputerUsers.Count == newComputerUsers.Count) return;
                if (computer.ComputerUsers == null) computer.ComputerUsers = newComputerUsers;
                foreach (var newComputerUser in newComputerUsers)
                {
                    var user = computer.ComputerUsers.FirstOrDefault(u => u.Name == newComputerUser.Name);
                    if (user == null)
                    {
                        computer.ComputerUsers.Add(newComputerUser);
                    }
                    else
                    {
                        var userChanged = user.FullName != newComputerUser.FullName
                                          || user.PrivilegeLevel != newComputerUser.PrivilegeLevel;

                        if (userChanged)
                        {
                            user.Synced = false;
                            user.FullName = newComputerUser.FullName;
                            user.PrivilegeLevel = newComputerUser.PrivilegeLevel;
                        }
                    }
                }

                var removedComputerUsers = computer.ComputerUsers.Except(newComputerUsers).ToList();
                removedComputerUsers.ForEach(u =>
                {
                    var removedComputerUser = computer.ComputerUsers.FirstOrDefault(cu => cu.Name == u.Name);
                    if (removedComputerUser != null)
                        removedComputerUser.Removed = true;
                });
                db.Computer = computer;
            }
            catch (Exception e)
            {
                LocalLogger.Log("UpdateUsers", e);
            }
        }
    }
}