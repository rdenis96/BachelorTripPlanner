using DataLayer.Context;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repository.Implementation
{
    public class TripsUsersRepository : ITripsUsersRepository
    {
        public TripUser Create(TripUser obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
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

        public List<TripUser> GetLazyByTripId(int tripId)
        {
            if (tripId < 0)
            {
                return null;
            }
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.TripUsers.Where(x => x.TripId == tripId).ToList();
                return result;
            }
        }

        public List<TripUser> GetByTripId(int tripId)
        {
            if (tripId < 0)
            {
                return null;
            }
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.TripUsers.Where(x => x.TripId == tripId).Include(x => x.User).ToList();
                return result;
            }
        }

        public bool IsUserAdmin(int userId, int tripId)
        {
            if (userId < 0 || tripId < 0)
            {
                return false;
            }

            using (TripPlanner context = new TripPlanner())
            {
                var tripUser = context.TripUsers.Where(x => x.UserId == userId && x.TripId == tripId).FirstOrDefault();
                if (tripUser != null)
                {
                    return tripUser.IsGroupAdmin;
                }

                return false;
            }
        }

        public List<TripUser> GetByUserId(int userId)
        {
            if (userId < 0)
            {
                return null;
            }
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.TripUsers.Where(x => x.UserId == userId && x.HasAcceptedInvitation == true).ToList();
                return result;
            }
        }

        public IEnumerable<TripUser> UpdateMany(IEnumerable<TripUser> tripUsers)
        {
            if (tripUsers == null)
                return null;

            var changesSaved = false;
            using (TripPlanner context = new TripPlanner())
            {
                foreach (var tripUser in tripUsers)
                {
                    context.Entry(tripUser).State = EntityState.Modified;
                }

                changesSaved = context.SaveChanges() > 0;

                return changesSaved ? tripUsers : Enumerable.Empty<TripUser>();
            }
        }

        public TripUser GetByUserIdAndTripId(int userId, int tripId)
        {
            if (userId < 0)
            {
                return null;
            }

            if (tripId < 0)
            {
                return null;
            }

            using (TripPlanner context = new TripPlanner())
            {
                var result = context.TripUsers.Where(x => x.UserId == userId && x.TripId == tripId).FirstOrDefault();
                return result;
            }
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