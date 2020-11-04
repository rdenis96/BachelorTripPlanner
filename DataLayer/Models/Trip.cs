using DataLayer.Enums;
using System;

namespace DataLayer.Models
{
    public class Trip : IEquatable<Trip>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TripType Type { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Trip);
        }

        public bool Equals(Trip other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Type);
        }
    }
}