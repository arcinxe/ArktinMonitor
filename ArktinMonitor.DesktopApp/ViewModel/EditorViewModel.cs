using ArktinMonitor.Data.ExtensionMethods;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ArktinMonitor.DesktopApp.ViewModel
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

        private BlockedApplicationDesktop _application;

        private void AppsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var computer = Db.Computer;
            var user = computer.ComputerUsers.FirstOrDefault(u => u.Name == User.Name);
            // if new itmes get added, attach change handlers to them
            if (e.NewItems != null)
            {
                foreach (BlockedApplicationDesktop app in e.NewItems)
                {
                    user?.BlockedApplications.Add(new BlockedApplicationLocal
                    {
                        Active = app.Active,
                        Name = app.Name,
                        Path = app.FilePath,
                        BlockedApplicationId = app.BlockedApplicationId,
                        Synced = false
                    });
                    app.PropertyChanged += OnAppPropertyChanged;
                }
            }

            // if items got removed, detach change handlers
            if (e.OldItems != null)
            {
                foreach (BlockedApplicationDesktop app in e.OldItems)
                {
                    var appToRemove = user?.BlockedApplications.FirstOrDefault(a => a.Path == app.FilePath);
                    user?.BlockedApplications.Remove(appToRemove);
                    app.PropertyChanged -= OnAppPropertyChanged;
                }
            }
            Db.Computer = computer;
        }

        private void OnAppPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var app = (BlockedApplicationDesktop)sender;
            var computer = Db.Computer;
            var user = computer.ComputerUsers.FirstOrDefault(u => u.Name == User.Name);
            var blockedApp = user?.BlockedApplications.FirstOrDefault(ba => ba.Path == app.TempFilePath);
            if (blockedApp != null)
            {
                blockedApp.Active = app.Active;
                blockedApp.Name = app.Name;
                blockedApp.Path = app.FilePath;
                blockedApp.BlockedApplicationId = app.BlockedApplicationId;
                blockedApp.Synced = false;
            }
            Db.Computer = computer;
        }

        public BlockedApplicationDesktop Application
        {
            get => _application;
            set
            {
                _application = value;
                RaisePropertyChangedEvent(nameof(Application));
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
            var app = new BlockedApplicationDesktop()
            {
                FilePath = dialog.FileName,
                Name = System.IO.Path.GetFileNameWithoutExtension(dialog.SafeFileName),
                Active = false,
                TempFilePath = dialog.FileName
            };
            if (User.BlockedApplications.Any(a => a.FilePath == app.FilePath))
            {
                MessageBox.Show("This app is already on the list");
                return;
            }

            User.BlockedApplications?.Add(app);
        }

        public ICommand RemoveBlockedAppCommand => new DelegateCommand(RemoveBlockedApp);

        public void RemoveBlockedApp()
        {
            User.BlockedApplications.Remove(Application);
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
            if (Application == null)
            {
                LocalLogger.Log("Application is null!");
                return;
            }
            if (dialog.FileName == Application.FilePath) return;
            if (User.BlockedApplications.Any(a => a.FilePath == dialog.FileName))
            {
                MessageBox.Show("This app is already on the list");
                return;
            }
            Application.TempFilePath = Application.FilePath;
            Application.Name = System.IO.Path.GetFileNameWithoutExtension(dialog.SafeFileName);
            Application.FilePath = dialog.FileName;
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
            var users = computer.ComputerUsers.Select(cu => cu.ToDesktopModel());
            Users.Clear();
            foreach (var user in users)
            {
                user.BlockedApplications.CollectionChanged += AppsOnCollectionChanged;
                foreach (var app in user.BlockedApplications)
                {
                    app.PropertyChanged += OnAppPropertyChanged;
                }
                Users.Add(user);
            }
            User = Users.FirstOrDefault();
        }
    }
}