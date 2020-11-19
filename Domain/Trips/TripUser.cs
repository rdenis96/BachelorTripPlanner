using Domain.Accounts;
using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Trips
{
    public class TripUser : UserTripBase, IEquatable<TripUser>
    {
        public User User { get; set; }
        public bool HasAcceptedInvitation { get; set; }
        public bool IsGroupAdmin { get; set; }
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TripUser);
        }

        public bool Equals(TripUser other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   UserId == other.UserId &&
                   TripId == other.TripId &&
                   EqualityComparer<User>.Default.Equals(User, other.User) &&
                   HasAcceptedInvitation == other.HasAcceptedInvitation &&
                   IsGroupAdmin == other.IsGroupAdmin &&
                   IsDeleted == other.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, UserId, TripId, User, HasAcceptedInvitation, IsGroupAdmin, IsDeleted);
        }
    }
}