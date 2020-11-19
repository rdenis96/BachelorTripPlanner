using Domain.Common;
using Domain.Trips.Enums;
using System;

namespace Domain.Trips
{
    public class Trip : BaseEntity, IEquatable<Trip>
    {
        public string Name { get; set; }
        public TripType Type { get; set; }
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Trip);
        }

        public bool Equals(Trip other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Type == other.Type &&
                   IsDeleted == other.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Name, Type, IsDeleted);
        }
    }
}