using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Context;
using DataLayer.Enums;
using DataLayer.Models;

namespace DataLayer.Repository.Implementation
{
    public class UserInterestRepository : IUserInterestRepository
    {
        public UserInterest Create(UserInterest obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.UserInterests.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(UserInterest obj)
        {
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                context.UserInterests.Remove(obj);
                if (context.UserInterests.Contains(obj))
                    return false;
                context.SaveChanges();
                return true;
            }
        }

        public ICollection<UserInterest> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserInterest GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserInterest GetByUserId(int userId)
        {
            if (userId <= 0)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var userInterest = context.UserInterests.Where(x => x.UserId == userId).FirstOrDefault();

                if (userInterest != null)
                {
                    return userInterest;
                }
                return null;
            }
        }

        public UserInterest Update(UserInterest obj)
        {
            if (obj == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var userInterest = context.UserInterests.Find(obj.UserId);
                if (userInterest == null)
                {
                    return null;
                }

                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.SaveChanges();

                return userInterest;
            }
        }
    }
}