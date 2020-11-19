using Domain.Common;
using System;

namespace Domain.Interests
{
    public class UserInterest : UserTripBase, IEquatable<UserInterest>
    {
        public string Countries { get; set; }
        public string Cities { get; set; }
        public string Weather { get; set; }
        public string TouristAttractions { get; set; }
        public string Transports { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserInterest);
        }

        public bool Equals(UserInterest other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   UserId == other.UserId &&
                   TripId == other.TripId &&
                   Countries == other.Countries &&
                   Cities == other.Cities &&
                   Weather == other.Weather &&
                   TouristAttractions == other.TouristAttractions &&
                   Transports == other.Transports;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Id);
            hash.Add(UserId);
            hash.Add(TripId);
            hash.Add(Countries);
            hash.Add(Cities);
            hash.Add(Weather);
            hash.Add(TouristAttractions);
            hash.Add(Transports);
            return hash.ToHashCode();
        }
    }
}