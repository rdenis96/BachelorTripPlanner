using DataLayer.Context;
using Domain.Repository;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Trips
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
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                var tripUser = context.TripUsers.Find(obj.Id);
                if (tripUser != null)
                {
                    tripUser.IsDeleted = true;
                    context.SaveChanges();
                }
                return true;
            }
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
                var result = context.TripUsers.Where(x => x.TripId == tripId && x.IsDeleted == false).ToList();
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
                var result = context.TripUsers.Where(x => x.TripId == tripId && x.IsDeleted == false).Include(x => x.User).ToList();
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
                var tripUser = context.TripUsers.Where(x => x.UserId == userId && x.TripId == tripId && x.IsDeleted == false).FirstOrDefault();
                if (tripUser != null)
                {
                    return tripUser.IsGroupAdmin;
                }

                return false;
            }
        }

        public List<TripUser> GetByUserId(int userId, bool includeDeleted = false)
        {
            if (userId < 0)
            {
                return null;
            }
            using (TripPlanner context = new TripPlanner())
            {
                var result = new List<TripUser>();
                if (includeDeleted)
                {
                    result = context.TripUsers.Where(x => x.UserId == userId && x.HasAcceptedInvitation == true).ToList();
                }
                else
                {
                    result = context.TripUsers.Where(x => x.UserId == userId && x.HasAcceptedInvitation == true && x.IsDeleted == false).ToList();
                }
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

        public int CountAdmins(int tripId)
        {
            if (tripId < 0)
            {
                return 0;
            }
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.TripUsers.Where(x => x.TripId == tripId && x.IsGroupAdmin && x.HasAcceptedInvitation && x.IsDeleted == false).Count();
                return result;
            }
        }

        public int CountTripUsers(int tripId)
        {
            if (tripId < 0)
            {
                return 0;
            }
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.TripUsers.Where(x => x.TripId == tripId && x.HasAcceptedInvitation && x.IsDeleted == false).Count();
                return result;
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
                var result = context.TripUsers.Where(x => x.UserId == userId && x.TripId == tripId && x.IsDeleted == false).FirstOrDefault();
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
                context.Entry(obj).State = EntityState.Modified;

                changesSaved = context.SaveChanges() > 0;

                return changesSaved ? GetByUserIdAndTripId(obj.UserId, obj.TripId.GetValueOrDefault()) : null;
            }
        }
    }
}