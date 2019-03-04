using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public class User : IEquatable<User>
    {
        public int Id { get; set; }

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
                   Id == other.Id &&
                   Email == other.Email &&
                   Password == other.Password &&
                   RegisterDate == other.RegisterDate &&
                   EqualityComparer<DateTime?>.Default.Equals(LastOnline, other.LastOnline) &&
                   Ip == other.Ip &&
                   Phone == other.Phone;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Email, Password, RegisterDate, LastOnline, Ip, Phone);
        }
    }
}