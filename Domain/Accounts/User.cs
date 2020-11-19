using Domain.Common;
using System;

namespace Domain.Accounts
{
    public class User : BaseEntity, IEquatable<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? LastOnline { get; set; }
        public string Ip { get; set; }
        public string Phone { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   Email == other.Email &&
                   Password == other.Password &&
                   RegisterDate == other.RegisterDate &&
                   LastOnline == other.LastOnline &&
                   Ip == other.Ip &&
                   Phone == other.Phone;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Email, Password, RegisterDate, LastOnline, Ip, Phone);
        }
    }
}