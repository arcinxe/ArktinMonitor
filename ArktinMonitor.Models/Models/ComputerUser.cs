using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ArktinMonitor.Data.Models
{
    public class ComputerUser : BasicComputerUser
    {
        public int ComputerUserId { get; set; }

        public bool Removed { get; set; }

        public int ComputerId { get; set; }

        public virtual Computer Computer { get; set; }
    }

    public class ComputerUserResource : BasicComputerUser
    {
        public int ComputerUserId { get; set; }

        public bool Removed { get; set; }

        public int ComputerId { get; set; }
    }

    public class ComputerUserLocal : BasicComputerUser, IEquatable<ComputerUserLocal>
    {
        public int ComputerUserId { get; set; }

        public bool Synced { get; set; }

        public bool Enabled { get; set; }

        public bool Removed { get; set; }

        public List<BlockedAppLocal> BlockedApps { get; set; }

        public List<BlockedSiteLocal> BlockedSites { get; set; }

        public List<DailyTimeLimitLocal> DailyTimeLimits { get; set; }

        public bool Equals(ComputerUserLocal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Removed == other.Removed;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ComputerUserLocal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Removed.GetHashCode();
                return hashCode;
            }
        }
    }

    public class ComputerUserDesktop : INotifyPropertyChanged
    {
        public int ComputerUserId { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChangedEvent(nameof(Name));
            }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                RaisePropertyChangedEvent(nameof(FullName));
            }
        }

        private string _privilegeLevel;

        public string PrivilegeLevel
        {
            get { return _privilegeLevel; }
            set
            {
                _privilegeLevel = value;
                RaisePropertyChangedEvent(nameof(PrivilegeLevel));
            }
        }

        private string _visibleName;

        public string VisibleName
        {
            get { return _visibleName; }
            set
            {
                _visibleName = value;
                RaisePropertyChangedEvent(nameof(VisibleName));
            }
        }

        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                RaisePropertyChangedEvent(nameof(Enabled));
            }
        }

        private bool _removed;

        public bool Removed
        {
            get { return _removed; }
            set
            {
                _removed = value;
                RaisePropertyChangedEvent(nameof(Removed));
            }
        }

        private ObservableCollection<BlockedAppDesktop> _blockedApps;

        public ObservableCollection<BlockedAppDesktop> BlockedApps
        {
            get { return _blockedApps; }
            set
            {
                _blockedApps = value;
                RaisePropertyChangedEvent(nameof(BlockedApps));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class BasicComputerUser : IEquatable<BasicComputerUser>
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string PrivilegeLevel { get; set; }

        public bool Equals(BasicComputerUser other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(FullName, other.FullName) && string.Equals(PrivilegeLevel, other.PrivilegeLevel);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BasicComputerUser)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PrivilegeLevel != null ? PrivilegeLevel.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}