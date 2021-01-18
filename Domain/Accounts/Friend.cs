using Domain.Common;
using System;

namespace Domain.Accounts
{
    public class Friend : BaseEntity, IEquatable<Friend>
    {
        public int UserId { get; set; }

        public int FriendId { get; set; }

        public User FriendAccount { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Friend);
        }

        public bool Equals(Friend other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   UserId == other.UserId &&
                   FriendId == other.FriendId &&
                   CreatedDate == other.CreatedDate &&
                   IsDeleted == other.IsDeleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, UserId, FriendId, CreatedDate, IsDeleted);
        }
    }
}