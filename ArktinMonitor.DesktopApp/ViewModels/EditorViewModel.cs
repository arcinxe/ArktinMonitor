using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ArktinMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.DesktopApp.ViewModels;
using ArktinMonitor.Data.Models;
using Microsoft.Win32;

namespace ArktinMonitor.DesktopApp.ViewModels
{
    internal class EditorViewModel : ViewModelTemplate
    {
        private static readonly JsonLocalDatabase Db = JsonLocalDatabase.Instance;

        public EditorViewModel()
        {
            Users = new ObservableCollection<ComputerUserDesktop>();
            Users.CollectionChanged += UsersOnCollectionChanged;
            
            RefreshUsers();
        }

        private BlockedAppDesktop _app;

        private void AppsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var computer = Db.Computer;
            var user = computer.ComputerUsers.FirstOrDefault(u => u.Name == User.Name);
            // if new itmes get added, attach change handlers to them
            if (e.NewItems != null)
            {
                foreach (BlockedAppDesktop app in e.NewItems)
                {
                    if (user.BlockedApps==null)
                    {
                        user.BlockedApps=new List<BlockedAppLocal>();
                    }
                    user?.BlockedApps.Add(new BlockedAppLocal
                    {
                        Active = app.Active,
                        Name = app.Name,
                        Path = app.FilePath,
                        BlockedAppId = app.BlockedAppId,
                        Synced = false
                    });
                    app.PropertyChanged += OnAppPropertyChanged;
                }
            }

            // if items got removed, detach change handlers
            if (e.OldItems != null)
            {
                foreach (BlockedAppDesktop app in e.OldItems)
                {
                    var appToRemove = user?.BlockedApps.FirstOrDefault(a => a.Path == app.FilePath);
                    user?.BlockedApps.Remove(appToRemove);
                    app.PropertyChanged -= OnAppPropertyChanged;
                }
            }
            Db.Computer = computer;
        }

        private void OnAppPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var app = (BlockedAppDesktop)sender;
            var computer = Db.Computer;
            var user = computer.ComputerUsers.FirstOrDefault(u => u.Name == User.Name);
            var blockedApp = user?.BlockedApps.FirstOrDefault(ba => ba.Path == app.TempFilePath);
            LocalLogger.Log("app");
            LocalLogger.Log(app);
            if (blockedApp != null)
            {
                blockedApp.Active = app.Active;
                blockedApp.Name = app.Name;
                blockedApp.Path = app.FilePath;
                blockedApp.BlockedAppId = app.BlockedAppId;
                blockedApp.Synced = false;
            }
            Db.Computer = computer;
        }

        public BlockedAppDesktop App
        {
            get => _app;
            set
            {
                _app = value;
                RaisePropertyChangedEvent(nameof(App));
            }
        }

        public ObservableCollection<ComputerUserDesktop> Users { get; set; }

        private ComputerUserDesktop _user;

        public ComputerUserDesktop User
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChangedEvent(nameof(User));
            }
        }

        public ICommand AddNewBlockedAppCommand => new DelegateCommand(AddNewBlockedApp);

        public void AddNewBlockedApp()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select app to block",
                Multiselect = false,
                DefaultExt = ".exe",
                Filter = "Executables (.exe)|*.exe"
            };
            var dlg = dialog.ShowDialog();
            if (dlg != true) return;
            var app = new BlockedAppDesktop()
            {
                FilePath = dialog.FileName,
                Name = System.IO.Path.GetFileNameWithoutExtension(dialog.SafeFileName),
                Active = false,
                TempFilePath = dialog.FileName
            };
            if (User.BlockedApps.Any(a => a.FilePath == app.FilePath))
            {
                MessageBox.Show("This app is already on the list");
                return;
            }
            User.BlockedApps?.Add(app);
        }

        public ICommand RemoveBlockedAppCommand => new DelegateCommand(RemoveBlockedApp);

        public void RemoveBlockedApp()
        {
            User.BlockedApps.Remove(App);
        }

        public ICommand EditBlockedAppCommand => new DelegateCommand(EditBlockedApp);

        public void EditBlockedApp()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select app to block",
                Multiselect = false,
                DefaultExt = ".exe",
                Filter = "Executables (.exe)|*.exe"
            };
            var dialogResult = dialog.ShowDialog();

            if (dialogResult == false) return;
            if (App == null)
            {
                LocalLogger.Log("Application is null!");
                return;
            }
            if (dialog.FileName == App.FilePath) return;
            if (User.BlockedApps.Any(a => a.FilePath == dialog.FileName))
            {
                MessageBox.Show("This app is already on the list");
                return;
            }
            App.TempFilePath = App.FilePath;
            App.Name = System.IO.Path.GetFileNameWithoutExtension(dialog.SafeFileName);
            App.FilePath = dialog.FileName;
        }

        private void UsersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // if new itmes get added, attach change handlers to them
            if (e.NewItems != null)
            {
                foreach (ComputerUserDesktop item in e.NewItems)
                {
                    item.PropertyChanged += ComputerUserDesktop_PropertyChanged;
                }
            }

            // if items got removed, detach change handlers
            if (e.OldItems != null)
            {
                foreach (ComputerUserDesktop item in e.OldItems)
                {
                    item.PropertyChanged -= ComputerUserDesktop_PropertyChanged;
                }
            }
            // Process Add/Remove here
        }

        private void ComputerUserDesktop_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            try
            {
                var user = (ComputerUserDesktop)sender;
                var computer = Db.Computer;
                var first = computer.ComputerUsers.FirstOrDefault(u => u.Name == user.Name);
                if (first != null) first.Enabled = user.Enabled;
                Db.Computer = computer;
            }
            catch (Exception e)
            {
                LocalLogger.Log(nameof(AddNewBlockedApp), e);
            }
        }

        public ICommand RefreshCommand => new DelegateCommand(RefreshUsers);

        public void RefreshUsers()
        {
            
            var computer = Db.Computer;
            if (computer.ComputerUsers==null) return;
            var tempUser = computer.ComputerUsers.FirstOrDefault();
            var tempUserModel = tempUser.ToDesktopModel();
            var users = computer.ComputerUsers.Select(cu => cu.ToDesktopModel()).ToList();

            Users.Clear();
            foreach (var user in users)
            {
                user.BlockedApps.CollectionChanged += AppsOnCollectionChanged;
                foreach (var app in user.BlockedApps)
                {
                    app.PropertyChanged += OnAppPropertyChanged;
                }
                Users.Add(user);
            }
            User = Users.FirstOrDefault();
        }
    }
}