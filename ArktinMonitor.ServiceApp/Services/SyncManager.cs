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
                SyncBlockedSites();
            }
            catch (Exception e)
            {
                LocalLogger.Log($"{nameof(SyncManager)} > {nameof(Credentials.GetJsonWebToken)}", e);
            }
            LocalLogger.Log($"Method {nameof(SyncData)} completed");
        }

        private static void SyncBlockedSites()
        {
            _computer = JsonLocalDatabase.Instance.Computer;
            var client = new ServerClient();
            var response = client.GetFromServer(Settings.ApiUrl, $"api/BlockedSites/{_computer.ComputerId}", _jsonWebToken);
            var returnSites = response.Content.ReadAsAsync<List<BlockedSiteResource>>().Result.ToList();
            foreach (var user in _computer.ComputerUsers)
            {
                user.BlockedSites.Clear();
                var userSites = returnSites.Where(rs => rs.ComputerUserId == user.ComputerUserId)
                    .Select(s => s.ToLocal());
                user.BlockedSites.AddRange(userSites);
            }
            JsonLocalDatabase.Instance.Computer = _computer;
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
                .Where(u => u.Synced && u.Enabled && u.ComputerUserId > 0 && u.BlockedApps.Any(a => !a.Synced)).ToList();
            var client = new ServerClient();
            if (users != null && users.Count > 0)
            {
                var apps = new List<BlockedAppLocal>();
                var appsResource = new List<BlockedAppResource>();
                foreach (var user in users)
                {
                    apps.AddRange(user.BlockedApps.Where(a => !a.Synced));
                    appsResource.AddRange(user.BlockedApps.Where(a => !a.Synced).Select(a => a.ToResource(user.ComputerUserId)));
                }
                if (apps.Count == 0) return;
                LocalLogger.Log($"Syncing {apps.Count} {(apps.Count == 1 ? "app" : "apps")}.");
                var response = client.PostToServer(Settings.ApiUrl, $"api/BlockedApps/{_computer.ComputerId}", appsResource, _jsonWebToken);
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
            }

            var syncResponse = client.GetFromServer(Settings.ApiUrl, $"api/BlockedApps/{_computer.ComputerId}", _jsonWebToken);
            var allLocalApps = new List<BlockedAppLocal>();
            if (_computer.ComputerUsers != null)
                foreach (var user in _computer.ComputerUsers)
                {
                    allLocalApps.AddRange(user.BlockedApps.Where(a => a.Synced));
                }
            var allApps = syncResponse.Content.ReadAsAsync<List<BlockedAppResource>>().Result.ToList();
            //LocalLogger.Log(allApps);
            var removedAppsIds = allLocalApps.Select(a => a.BlockedAppId).ToList();
            foreach (var app in allApps)
            {
                var user = _computer.ComputerUsers?.FirstOrDefault(u => u.ComputerUserId == app.ComputerUserId);
                if (user == null) continue;
                if (user.BlockedApps.All(a => a.Path != app.Path))
                {
                    user.BlockedApps.Add(new BlockedAppLocal
                    {
                        Active = app.Active,
                        BlockedAppId = app.BlockedAppId,
                        Name = app.Name,
                        Path = app.Path,
                        Synced = true
                    });
                }
                else
                {
                    var oldApp = user.BlockedApps.FirstOrDefault(a => a.Path == app.Path);
                    if (oldApp == null) continue;
                    oldApp.Name = app.Name;
                    oldApp.Path = app.Path;
                    oldApp.Active = app.Active;
                    oldApp.Synced = true;
                    oldApp.BlockedAppId = app.BlockedAppId;
                }
                removedAppsIds.RemoveAll(i => i == app.BlockedAppId);
            }

            // Removes locally all blocked apps removed via web app.
            if (_computer.ComputerUsers != null)
                foreach (var user in _computer.ComputerUsers)
                {
                    user.BlockedApps.RemoveAll(a => removedAppsIds.Contains(a.BlockedAppId));
                }

            //LocalLogger.Log(returnApps);
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