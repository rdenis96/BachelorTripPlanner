using System;

namespace DataLayer.Models
{
    public class User : IEquatable<User>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((User)obj);
        }

        public bool Equals(User obj)
        {
            if (obj == null)
                return false;

            var result = (Id == obj.Id) &&
                         (Email.Equals(obj.Email)) &&
                         (Password.Equals(obj.Password));
            return result;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}