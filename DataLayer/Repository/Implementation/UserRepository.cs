using DataLayer.Context;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository.Implementation
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
                return context.Users.Where(c => c.Email == email && c.Password == password).FirstOrDefault();
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
                User oldUser = context.Users.Find(obj.Id);
                if (oldUser != null)
                {
                    oldUser.Email = obj.Email;
                    oldUser.Password = obj.Password;
                }
                context.SaveChanges();
                return context.Users.Find(obj.Id);
            }
        }
    }
}