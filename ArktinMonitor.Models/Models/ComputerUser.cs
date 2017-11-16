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

    public class ComputerUserLocal : BasicComputerUser, IEquatable<ComputerUserLocal>
    {
        public int ComputerUserId { get; set; }

        public bool Synced { get; set; }

        public bool Hidden { get; set; }

        public bool Removed { get; set; }

        public List<BlockedApplicationLocal> BlockedApplications { get; set; }

        public List<BlockedSiteLocal> BlockedSites { get; set; }

        public bool Equals(ComputerUserLocal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Hidden == other.Hidden && Removed == other.Removed;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComputerUserLocal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Hidden.GetHashCode();
                hashCode = (hashCode * 397) ^ Removed.GetHashCode();
                return hashCode;
            }
        }
    }

    public class ComputerUserDesktop : BasicComputerUser, INotifyPropertyChanged
    {
        public string VisibleName { get; set; }

        public bool Enabled { get; set; }

        //public bool Removed { get; set; }

        private ObservableCollection<BlockedApplicationDesktop> _blockedApplications;
        public ObservableCollection<BlockedApplicationDesktop> BlockedApplications {
            get { return _blockedApplications; }
            set
            {
                _blockedApplications = value;
                RaisePropertyChangedEvent(nameof(BlockedApplications));
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicComputerUser)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PrivilegeLevel != null ? PrivilegeLevel.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
