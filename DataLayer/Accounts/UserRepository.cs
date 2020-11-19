using DataLayer.Context;
using Domain.Accounts;
using Domain.Repository;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Accounts
{
    public class UserRepository : IUserRepository
    {
        public User Create(User obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.Users.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public ICollection<User> GetAll()
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Users.ToList();
            }
        }

        //public ICollection<User> GetAllOnlineBetweenDates(DateTime startDate, DateTime endDate)
        //{
        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        return context.Users.Where(c => c.Lastonline >= startDate && c.Lastonline <= endDate).ToList();
        //    }
        //}

        //public ICollection<User> GetAllRegisteredBetweenDates(DateTime startDate, DateTime endDate)
        //{
        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        return context.Users.Where(c => c.Registerdate >= startDate && c.Registerdate <= endDate).ToList();
        //    }
        //}

        public User GetByEmail(string email)
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Users.Where(c => c.Email == email).FirstOrDefault();
            }
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            using (TripPlanner context = new TripPlanner())
            {
                var passwordHash = UserHelper.MD5Hash(password);
                return context.Users.Where(c => c.Email == email && c.Password == passwordHash).FirstOrDefault();
            }
        }

        //public ICollection<User> GetByFirstname(string firstname)
        //{
        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        return context.Users.Where(c => c.FirstName == firstname).ToList();
        //    }
        //}

        public User GetById(int id)
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Users.Where(c => c.Id == id).FirstOrDefault();
            }
        }

        //public ICollection<User> GetByIp(string ip)
        //{
        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        return context.Users.Where(c => c.Ip == ip).ToList();
        //    }
        //}

        //public ICollection<User> GetByLastname(string lastname)
        //{
        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        return context.Users.Where(c => c.LastName == lastname).ToList();
        //    }
        //}

        //public User GetByPhone(string phone)
        //{
        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        return context.Users.Where(c => c.Phone == phone).FirstOrDefault();
        //    }
        //}

        public bool Delete(User obj)
        {
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                context.Users.Remove(obj);
                if (context.Users.Contains(obj))
                    return false;
                context.SaveChanges();
                return true;
            }
        }

        public User Update(User obj)
        {
            if (obj == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.SaveChanges();

                var result = context.Users.Find(obj.Id);
                return result;
            }
        }

        public List<int> GetIdsByEmail(List<string> emails)
        {
            if (emails == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                List<int> ids = new List<int>();
                foreach (var email in emails)
                {
                    var user = context.Users.Where(x => x.Email == email).FirstOrDefault();
                    if (user != null)
                    {
                        ids.Add(user.Id);
                    }
                }
                return ids;
            }
        }
    }
}