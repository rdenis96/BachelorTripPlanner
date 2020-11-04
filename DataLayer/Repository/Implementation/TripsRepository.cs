using DataLayer.Context;
using DataLayer.Enums;
using DataLayer.Models;
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
            throw new NotImplementedException();
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