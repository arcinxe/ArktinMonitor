using System;
using System.ComponentModel;

namespace ArktinMonitor.Data.Models
{
    public class BlockedApp : BasicBlockedApp
    {
        public int BlockedAppId { get; set; }

        public int ComputerUserId { get; set; }

        public virtual ComputerUser ComputerUser { get; set; }
    }

    public class BlockedAppLocal : BasicBlockedApp
    {
        public int BlockedAppId { get; set; }

        public bool Synced { get; set; }
    }

    public class BlockedAppResource:BasicBlockedApp
    {
        public int BlockedAppId { get; set; }

        public int ComputerUserId { get; set; }
    }

    public class BlockedAppDesktop : INotifyPropertyChanged
    {
        private int _blockedAppId;

        public int BlockedAppId
        {
            get { return _blockedAppId; }
            set
            {
                _blockedAppId = value;
                RaisePropertyChangedEvent(nameof(BlockedAppId));
            }
        }

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

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChangedEvent(nameof(FilePath));
            }
        }

        private string _tempfilePath;

        public string TempFilePath
        {
            get { return _tempfilePath; }
            set
            {
                _tempfilePath = value;
                RaisePropertyChangedEvent(nameof(TempFilePath));
            }
        }

        private bool _active;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                RaisePropertyChangedEvent(nameof(Active));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class BasicBlockedApp : IEquatable<BasicBlockedApp>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public bool Active { get; set; }

        public bool Equals(BasicBlockedApp other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicBlockedApp)obj);
        }

        public override int GetHashCode()
        {
            return Path?.GetHashCode() ?? 0;
        }
    }
}