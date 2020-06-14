using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Trip : IEquatable<Trip>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserInterestWrapper Interests { get; set; }
        public TripType Type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((Trip)obj);
        }

        public bool Equals(Trip obj)
        {
            var trip = obj as Trip;
            return trip != null &&
                   Id == trip.Id &&
                   Name == trip.Name &&
                   EqualityComparer<UserInterestWrapper>.Default.Equals(Interests, trip.Interests) &&
                   Type == trip.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Interests, Type);
        }
    }
}