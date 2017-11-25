using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class SyncManager
    {
        private static ComputerLocal _computer;

        public static void SyncData()
        {
            LocalLogger.Log($"Method {nameof(SyncData)} is running");
            SyncComputer();
            SyncDisks();
            SyncUsers();
            LocalLogger.Log($"Method {nameof(SyncData)} has stopped");
        }

        private static void SyncComputer()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            if (_computer.Synced) return;
            var client = new ServerClient();
            var response = client.PostToServer(Settings.ApiUrl, "api/Computers", _computer.ToResourceModel(), Settings.TempBearerToken);
            if (!response.IsSuccessStatusCode) return;
            var computerId = response.Content.ReadAsAsync<int>().Result;
            _computer.Synced = true;
            _computer.ComputerId = computerId;
            JsonLocalDatabase.Instance.Computer = _computer;
            //LocalLogger.Log($"ComputerId: {computerId}");
        }

        private static void SyncDisks()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            var disks = _computer.Disks.Where(d => !d.Synced).Select(d => d.ToResourceModel(_computer.ComputerId)).ToList();
            if (disks.Count == 0) return;
            LocalLogger.Log($"Syncing {disks.Count} {(disks.Count > 1 ? "disks" : "disk")}.");
            var client = new ServerClient();
            var response = client.PostToServer(Settings.ApiUrl, "api/Disks", disks, Settings.TempBearerToken);
            //LocalLogger.Log(response);
            if (!response.IsSuccessStatusCode) return;
            var returnDisks = response.Content.ReadAsAsync<List<DiskResourceModel>>().Result.Select(d => d.ToLocal()).ToList();
            _computer.Disks.RemoveAll(d => !d.Synced);
            _computer.Disks.AddRange(returnDisks);
            JsonLocalDatabase.Instance.Computer = _computer;
        }

        private static void SyncUsers()
        {
        }
    }
}