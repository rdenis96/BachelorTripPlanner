using System;

namespace Domain.Common
{
    public class BaseEntity : IEquatable<BaseEntity>
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public bool Equals(BaseEntity other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}