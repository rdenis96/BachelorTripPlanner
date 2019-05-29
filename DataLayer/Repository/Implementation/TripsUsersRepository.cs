using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Context;
using DataLayer.Models;

namespace DataLayer.Repository.Implementation
{
    public class TripsUsersRepository : ITripsUsersRepository
    {
        public TripUser Create(TripUser obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                UserInterestWrapper interests = new UserInterestWrapper(string.Empty);
                obj.Interests = interests;
                context.TripUsers.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(TripUser obj)
        {
            throw new NotImplementedException();
        }

        public ICollection<TripUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public TripUser GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TripUser> GetByTripId(int tripId)
        {
            throw new NotImplementedException();
        }

        public List<TripUser> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public TripUser GetByUserIdAndTripId(int userId, int tripId)
        {
            throw new NotImplementedException();
        }

        public TripUser Update(TripUser obj)
        {
            if (obj == null)
                return null;

            var changesSaved = false;
            using (TripPlanner context = new TripPlanner())
            {
                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                changesSaved = context.SaveChanges() > 0;

                return changesSaved ? GetByUserIdAndTripId(obj.UserId, obj.TripId) : null;
            }
        }
    }
}