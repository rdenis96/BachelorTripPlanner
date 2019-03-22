using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Models
{
    public class UserInterest : IEquatable<UserInterest>
    {
        public int UserId { get; set; }
        public string Countries { get; set; }
        public string Cities { get; set; }
        public WeathersEnum Weathers { get; set; }
        public string TouristAttractions { get; set; }
        public TransportsEnum Transports { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserInterest);
        }

        public bool Equals(UserInterest other)
        {
            return other != null &&
                   UserId == other.UserId &&
                   Countries == other.Countries &&
                   Cities == other.Cities &&
                   Weathers == other.Weathers &&
                   TouristAttractions == other.TouristAttractions &&
                   Transports == other.Transports;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, Countries, Cities, Weathers, TouristAttractions, Transports);
        }
    }
}