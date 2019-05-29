using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Context;
using DataLayer.Enums;
using DataLayer.Models;

namespace DataLayer.Repository.Implementation
{
    public class TripsRepository : ITripsRepository
    {
        public Trip Create(Trip obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                UserInterestWrapper interests = new UserInterestWrapper(string.Empty);
                obj.Interests = interests;
                context.Trips.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(Trip obj)
        {
            throw new NotImplementedException();
        }

        public ICollection<Trip> GetAll()
        {
            throw new NotImplementedException();
        }

        public Trip GetById(int id)
        {
            throw new NotImplementedException();
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