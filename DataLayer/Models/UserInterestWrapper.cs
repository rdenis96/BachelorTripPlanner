using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class UserInterestWrapper : IEquatable<UserInterestWrapper>
    {
        public string Countries { get; set; }
        public string Cities { get; set; }
        public string Weather { get; set; }
        public string TouristAttractions { get; set; }
        public string Transports { get; set; }

        public UserInterestWrapper()
        {
        }

        public UserInterestWrapper(string initValue)
        {
            Countries = initValue;
            Cities = initValue;
            Weather = initValue;
            TouristAttractions = initValue;
            Transports = initValue;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserInterestWrapper);
        }

        public bool Equals(UserInterestWrapper other)
        {
            return other != null &&
                   Countries == other.Countries &&
                   Cities == other.Cities &&
                   Weather == other.Weather &&
                   TouristAttractions == other.TouristAttractions &&
                   Transports == other.Transports;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Countries, Cities, Weather, TouristAttractions, Transports);
        }
    }
}