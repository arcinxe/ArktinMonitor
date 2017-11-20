using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using Microsoft.Win32;
using ArktinMonitor.Data.ExtensionMethods;
using MaterialDesignThemes.Wpf;

namespace ArktinMonitor.DesktopApp.ViewModel
{
    internal class EditorViewModel : ViewModelTemplate
    {
        private BlockedApplicationDesktop _application;

        public EditorViewModel()
        {
            Users = new ObservableCollection<ComputerUserDesktop>();
            RefreshUsers();
            //User = new ComputerUserDesktop() { BlockedApplications = new ObservableCollection<BlockedApplicationDesktop>() };
            //Users.CollectionChanged += UsersOnCollectionChanged;
            //User.BlockedApplications.CollectionChanged += BlockedApplicationsOnCollectionChanged;
            //BlockedApps.CollectionChanged += BlockedApps_CollectionChanged;
        }

        private void BlockedApps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LocalLogger.Log("another thing changed");
        }

        private void BlockedApplicationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            LocalLogger.Log("something changed");
        }

        //private BlockedApplicationDesktop _blockedApplication;

        public BlockedApplicationDesktop Application
        {
            get { return _application; }
            set
            {
                _application = value;
                RaisePropertyChangedEvent(nameof(Application));
            }
        }

        //private ObservableCollection<BlockedApplicationDesktop> _blockedApplications;
        //public ObservableCollection<BlockedApplicationDesktop> BlockedApps
        //{
        //    get { return User.BlockedApplications; }
        //    set
        //    {
        //        User.BlockedApplications = value;
        //        RaisePropertyChangedEvent(nameof(User.BlockedApplications));
        //        RaisePropertyChangedEvent(nameof(BlockedApps));
        //    }
        //}


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
                Active = false
            };
            if (User.BlockedApplications.Any(a => a.FilePath == app.FilePath))
            {
                MessageBox.Show("This app is already on the list");
                return;
            }

            User.BlockedApplications?.Add(app);
        }

        public ICommand EditBlockedAppCommand => new DelegateCommand(EditBlockedApp);
        public void EditBlockedApp()
        {
            var dialog = new OpenFileDialog
            {
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
            Application.Name = System.IO.Path.GetFileNameWithoutExtension(dialog.SafeFileName);
            Application.FilePath = dialog.FileName;
        }


        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LocalLogger.Log("Event is working");
        }

        void Users_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
        }

        public ICommand WindowLoadedCommand => new DelegateCommand(WindowLoaded);

        public void WindowLoaded()
        {
            LocalLogger.Log("window loaded");
        }

        private void UsersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // if new itmes get added, attach change handlers to them
            if (e.NewItems != null)
            {
                foreach (ComputerUserDesktop item in e.NewItems)
                {
                    item.PropertyChanged += ComputerUserDesktop_PropertyChanged;
                    LocalLogger.Log("added");
                }
            }

            // if items got removed, detach change handlers
            if (e.OldItems != null)
            {
                foreach (ComputerUserDesktop item in e.OldItems)
                {
                    item.PropertyChanged -= ComputerUserDesktop_PropertyChanged;
                    LocalLogger.Log("removed");
                }
            }


            LocalLogger.Log("Event is working");
            LocalLogger.Log($"Event is working");
            // Process Add/Remove here
        }

        private void ComputerUserDesktop_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Process Update here
            throw new NotImplementedException();
        }




        public void RefreshUsers()
        {
            var db = JsonLocalDatabase.Instance;
            var computer = db.Computer;
            var users = db.Computer.ComputerUsers.Select(cu => cu.ToDesktopModel());
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
            User = Users.FirstOrDefault();
            //var users = computer.ComputerUsers
            //    .Where(cu => !cu.Removed)
            //    .Select(u => new ComputerUserDesktop { FullName = u.FullName, Name = u.Name, Enabled = !u.Enabled, Removed = u.Removed, BlockedApps = u.BlockedApps }).ToList();

            //users.ForEach(u => u.VisibleName = string.IsNullOrWhiteSpace(u.FullName) ? u.Name : u.FullName);
            //new ObservableCollection<ComputerUserDesktop>(users);
        }

        public void SaveChanges()
        {
        }
    }
}
