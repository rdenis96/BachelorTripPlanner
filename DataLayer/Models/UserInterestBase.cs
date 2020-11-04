using System;

namespace DataLayer.Models
{
    public class UserInterestBase : IEquatable<UserInterestBase>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TripId { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserInterestBase);
        }

        public bool Equals(UserInterestBase other)
        {
            return other != null &&
                   Id == other.Id &&
                   UserId == other.UserId &&
                   TripId == other.TripId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserId, TripId);
        }
    }
}