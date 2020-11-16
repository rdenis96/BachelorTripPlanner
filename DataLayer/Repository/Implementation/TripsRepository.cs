using DataLayer.Context;
using DataLayer.Enums;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repository.Implementation
{
    public class TripsRepository : ITripsRepository
    {
        public Trip Create(Trip obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.Trips.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(Trip obj)
        {
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                var trip = context.Trips.Find(obj.Id);
                if (trip != null)
                {
                    trip.IsDeleted = true;

                    var pendingInvitedUsers = context.TripUsers.Where(x => x.TripId == trip.Id && x.HasAcceptedInvitation == false);
                    pendingInvitedUsers.ForEachAsync(x => x.IsDeleted = true);

                    context.SaveChanges();
                }
                return true;
            }
        }

        public ICollection<Trip> GetAll()
        {
            throw new NotImplementedException();
        }

        public Trip GetById(int id)
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Trips.Where(c => c.Id == id).FirstOrDefault();
            }
        }

        public List<Trip> GetByType(TripType type)
        {
            throw new NotImplementedException();
        }

        public Trip Update(Trip obj)
        {
            if (obj == null)
                return null;

            var changesSaved = false;
            using (TripPlanner context = new TripPlanner())
            {
                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                changesSaved = context.SaveChanges() > 0;

                return changesSaved ? GetById(obj.Id) : null;
            }
        }
    }
}