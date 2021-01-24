using Domain.Common;
using System;

namespace Domain.Interests
{
    public class SimilarInterest : BaseEntity, IEquatable<SimilarInterest>
    {
        public int InterestId { get; set; }

        public Interest Interest { get; set; }

        public int SimInterestId { get; set; }

        public Interest SimInterest { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SimilarInterest);
        }

        public bool Equals(SimilarInterest other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   InterestId == other.InterestId &&
                   SimInterestId == other.SimInterestId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, InterestId, SimInterestId);
        }
    }
}