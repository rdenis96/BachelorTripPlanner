using DataLayer.Context;
using Domain.Interests;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Interests
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
                var userInterests = context.UserInterests.Find(obj.Id);
                if (userInterests != null)
                {
                    context.Entry(userInterests).State = EntityState.Deleted;
                    return context.SaveChanges() > 0;
                }

                return false;
            }
        }

        public ICollection<UserInterest> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserInterest GetById(int id)
        {
            if (id < 0)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var interest = context.UserInterests.Find(id);

                if (interest != null)
                {
                    return interest;
                }
                return null;
            }
        }

        public UserInterest GetByUserId(int userId)
        {
            if (userId <= 0)
                return null;

            UserInterest userInterest = new UserInterest();
            using (TripPlanner context = new TripPlanner())
            {
                userInterest = context.UserInterests.Where(x => x.UserId == userId && x.TripId == null).FirstOrDefault();
                return userInterest;
            }
        }

        public UserInterest GetByUserIdAndTripId(int userId, int tripId)
        {
            if (userId <= 0)
                return null;

            var userInterest = new UserInterest();
            using (TripPlanner context = new TripPlanner())
            {
                userInterest = context.UserInterests.Where(x => x.UserId == userId && x.TripId == tripId).FirstOrDefault();
                return userInterest;
            }
        }

        public IEnumerable<UserInterest> GetByTripId(int tripId)
        {
            if (tripId <= 0)
                return null;

            var userInterest = Enumerable.Empty<UserInterest>();
            using (TripPlanner context = new TripPlanner())
            {
                userInterest = context.UserInterests.Where(x => x.TripId == tripId);
                return userInterest;
            }
        }

        public UserInterest Update(UserInterest obj)
        {
            if (obj == null)
                return null;

            var changesSaved = false;
            using (TripPlanner context = new TripPlanner())
            {
                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                changesSaved = context.SaveChanges() > 0;
            }
            return changesSaved ? GetById(obj.Id) : null;
        }
    }
}