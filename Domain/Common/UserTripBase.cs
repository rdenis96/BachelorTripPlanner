using System;

namespace Domain.Common
{
    public class UserTripBase : BaseEntity, IEquatable<UserTripBase>
    {
        public int UserId { get; set; }
        public int? TripId { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserTripBase);
        }

        public bool Equals(UserTripBase other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   UserId == other.UserId &&
                   TripId == other.TripId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, UserId, TripId);
        }
    }
}