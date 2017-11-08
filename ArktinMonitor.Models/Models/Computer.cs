using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArktinMonitor.Data.Models
{
    public class Computer : BasicComputer
    {
        public int ComputerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        public new double Ram { get; set; }

        public int WebAccountId { get; set; }

        public virtual WebAccount WebAccount { get; set; }
    }

    public class ComputerViewModel : BasicComputer
    {
        public List<Disk> Disks { get; set; }

        public List<ComputerUser> ComputerUsers { get; set; }

        public List<LogTimeInterval> LogTimeIntervals { get; set; }
    }

    public class ComputerResourceModel : BasicComputer
    {
        public int ComputerId { get; set; }
    }

    public class ComputerLocal : BasicComputer
    {

        public bool Synced { get; set; }

        public List<DiskLocal> Disks { get; set; }

        public List<ComputerUserLocal> ComputerUsers { get; set; }

        public List<LogTimeInterval> LogTimeIntervals { get; set; }



        //// Override of the == operator.
        //public static bool operator ==(ComputerLocal lhs, ComputerLocal rhs)
        //{
        //    if (rhs == null || lhs == null)
        //    {
        //        return false;
        //    }

        //    var status = lhs.Name == rhs.Name
        //     && lhs.Cpu == rhs.Cpu
        //     && lhs.Gpu == rhs.Gpu
        //     && lhs.OperatingSystem == rhs.OperatingSystem
        //     && Math.Abs(lhs.Ram - rhs.Ram) < 0.10
        //     && lhs.MacAddress == rhs.MacAddress;

        //    return status;
        //}

        //public static bool operator !=(ComputerLocal lhs, ComputerLocal rhs)
        //{
        //    return !(lhs == rhs);
        //}

        //protected bool Equals(ComputerLocal other)
        //{
        //    return Synced == other.Synced && Equals(Disks, other.Disks) && Equals(ComputerUsers, other.ComputerUsers) && Equals(LogTimeIntervals, other.LogTimeIntervals);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    return obj.GetType() == this.GetType() && Equals((ComputerLocal)obj);
        //}

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        var hashCode = Synced.GetHashCode();
        //        hashCode = (hashCode * 397) ^ (Disks?.GetHashCode() ?? 0);
        //        hashCode = (hashCode * 397) ^ (ComputerUsers?.GetHashCode() ?? 0);
        //        hashCode = (hashCode * 397) ^ (LogTimeIntervals?.GetHashCode() ?? 0);
        //        return hashCode;
        //    }
        //}
    }

    public abstract class BasicComputer
    {
        public string Name { get; set; }

        public string Cpu { get; set; }

        public string Gpu { get; set; }

        public double Ram { get; set; }

        public string OperatingSystem { get; set; }

        public string MacAddress { get; set; }
    }
}
