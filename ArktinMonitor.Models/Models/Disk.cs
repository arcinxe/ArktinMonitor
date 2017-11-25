using System;

namespace ArktinMonitor.Data.Models
{
    public class Disk : BasicDisk
    {
        public int DiskId { get; set; }

        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }

    public class DiskResourceModel : BasicDisk
    {
        public int DiskId { get; set; }

        public int ComputerId { get; set; }
    }

    public class DiskLocal : BasicDisk, IEquatable<DiskLocal>
    {
        public int DiskId { get; set; }

        public bool Synced { get; set; }

        public bool Equals(DiskLocal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Letter, other.Letter) && string.Equals(Name, other.Name) && TotalSpaceInGigaBytes.Equals(other.TotalSpaceInGigaBytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DiskLocal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Letter != null ? Letter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TotalSpaceInGigaBytes.GetHashCode();
                return hashCode;
            }
        }
    }

    public abstract class BasicDisk : IEquatable<BasicDisk>
    {
        public string Letter { get; set; }
        public string Name { get; set; }
        public double TotalSpaceInGigaBytes { get; set; }
        public double FreeSpaceInGigaBytes { get; set; }

        public bool Equals(BasicDisk other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Letter, other.Letter) && string.Equals(Name, other.Name) && TotalSpaceInGigaBytes.Equals(other.TotalSpaceInGigaBytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicDisk)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Letter != null ? Letter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TotalSpaceInGigaBytes.GetHashCode();
                return hashCode;
            }
        }
    }
}