using System;

namespace DataLayer.Models
{
    public class UserInterest : UserInterestBase, IEquatable<UserInterest>
    {
        public string Countries { get; set; }
        public string Cities { get; set; }
        public string Weather { get; set; }
        public string TouristAttractions { get; set; }
        public string Transports { get; set; }

        public UserInterest() : base()
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserInterest);
        }

        public bool Equals(UserInterest other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Countries == other.Countries &&
                   Cities == other.Cities &&
                   Weather == other.Weather &&
                   TouristAttractions == other.TouristAttractions &&
                   Transports == other.Transports;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Countries, Cities, Weather, TouristAttractions, Transports);
        }
    }
}