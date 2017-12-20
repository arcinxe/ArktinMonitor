using System;
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
        private static readonly CredentialsManager Credentials = new CredentialsManager(Settings.ApiUrl, Settings.UserRelatedStoragePath, Settings.SystemRelatedStoragePath, "ArktinMonitor");
        private static string _jsonWebToken;
        public static void SyncData()
        {
            LocalLogger.Log($"Method {nameof(SyncData)} is running");
            try
            {
                _jsonWebToken = Credentials.GetJsonWebToken();
                if (string.IsNullOrWhiteSpace(_jsonWebToken))
                {
                    LocalLogger.Log($"[{nameof(SyncData)}] no web api authentication");
                }
                SyncComputer();
                SyncDisks();
                SyncUsers();
                SyncIntervalTimeLogs();
                SyncBlockedApps();
            }
            catch (Exception e)
            {
                LocalLogger.Log($"{nameof(SyncManager)} > {nameof(Credentials.GetJsonWebToken)}", e);
            }
            LocalLogger.Log($"Method {nameof(SyncData)} completed");
        }

        private static void SyncComputer()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            if (_computer.Synced) return;
            var client = new ServerClient();
            var response = client.PostToServer(Settings.ApiUrl, "api/Computers", _computer.ToResourceModel(), _jsonWebToken);
            if (!response.IsSuccessStatusCode)
            {
                LocalLogger.Log(response.Content.ReadAsStringAsync());
                return;
            }

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
            var response = client.PostToServer(Settings.ApiUrl, "api/Disks", disks, _jsonWebToken);
            //LocalLogger.Log(response);
            if (!response.IsSuccessStatusCode) return;
            var returnDisks = response.Content.ReadAsAsync<List<DiskResource>>().Result.Select(d => d.ToLocal()).ToList();
            _computer.Disks.RemoveAll(d => !d.Synced);
            _computer.Disks.AddRange(returnDisks);
            JsonLocalDatabase.Instance.Computer = _computer;
        }

        private static void SyncUsers()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            var users = _computer.ComputerUsers?
                .Where(u => !u.Synced && u.Enabled)
                .Select(u => u.ToResource(_computer.ComputerId)).ToList();
            if (users == null || users.Count == 0) return;
            LocalLogger.Log($"Syncing {users.Count} {(users.Count > 1 ? "users" : "user")}.");
            var client = new ServerClient();
            var response = client.PostToServer(Settings.ApiUrl, "api/ComputerUsers", users, _jsonWebToken);
            if (!response.IsSuccessStatusCode) return;
            //LocalLogger.Log(response);
            var returnUsers = response.Content.ReadAsAsync<List<ComputerUserResource>>().Result.Select(d => d.ToLocal()).ToList();
            foreach (var user in returnUsers)
            {
                var oldUser = _computer.ComputerUsers.FirstOrDefault(u => u.Name == user.Name);
                if (oldUser == null) continue;
                oldUser.Synced = true;
                oldUser.ComputerUserId = user.ComputerUserId;
            }
            JsonLocalDatabase.Instance.Computer = _computer;
        }

        private static void SyncBlockedApps()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            var users = _computer.ComputerUsers?
                .Where(u => u.Synced && u.Enabled && u.ComputerUserId > 0).ToList();
            if (users == null || users.Count == 0) return;
            var apps = new List<BlockedAppLocal>();
            var appsResource = new List<BlockedAppResource>();
            foreach (var user in users)
            {
                apps.AddRange(user.BlockedApps.Where(a => !a.Synced));
                appsResource.AddRange(user.BlockedApps.Where(a => !a.Synced).Select(a => a.ToResource(user.ComputerUserId)));
            }
            if (apps.Count == 0) return;
            LocalLogger.Log($"Syncing {apps.Count} {(apps.Count > 1 ? "apps" : "app")}.");
            var client = new ServerClient();
            var response = client.PostToServer(Settings.ApiUrl, "api/BlockedApps", appsResource, _jsonWebToken);
            var returnApps = response.Content.ReadAsAsync<List<BlockedAppResource>>().Result.ToList();
            if (!response.IsSuccessStatusCode) return;
            foreach (var user in users)
            {
                if (user.BlockedApps == null) continue;
                foreach (var app in user.BlockedApps)
                {
                    var tempApp = returnApps.FirstOrDefault(a => a.Path == app.Path && a.ComputerUserId == user.ComputerUserId);
                    if (tempApp == null) continue;
                    app.BlockedAppId = tempApp.BlockedAppId;
                    app.Synced = true;
                }
            }
            LocalLogger.Log(returnApps);
            JsonLocalDatabase.Instance.Computer = _computer;
        }

        private static void SyncIntervalTimeLogs()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            //var users = _computer.ComputerUsers?
            //    .Where(u => u.Synced && u.Enabled && u.ComputerUserId > 0).ToList();
            var intervals = _computer.LogTimeIntervals.Where(l => !l.Synced).ToList();
            if (intervals.Count == 0) return;
            var intervalsResource = intervals.Select(i => i.ToResource(_computer.ComputerId, i.ComputerUser));
            //LocalLogger.Log(intervalsResource);
            var client = new ServerClient();
            var response = client.PostToServer(Settings.ApiUrl, "api/LogTimeIntervals", intervalsResource, _jsonWebToken);
            var returnLogs = response.Content.ReadAsAsync<List<LogTimeIntervalResource>>().Result.ToList();
            //LocalLogger.Log(returnLogs);
            foreach (var log in returnLogs)
            {
                var interval = intervals.FirstOrDefault(i => i.StartTime == log.StartTime);
                if (interval != null)
                {
                    interval.LogTimeIntervalId = log.LogTimeIntervalId;
                    interval.Synced = true;
                }
            }
            JsonLocalDatabase.Instance.Computer = _computer;

        }
    }
}