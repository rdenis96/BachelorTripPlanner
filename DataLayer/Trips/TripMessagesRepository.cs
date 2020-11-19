using DataLayer.Context;
using Domain.Repository;
using Domain.Trips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Trips
{
    public class TripMessagesRepository : ITripMessagesRepository
    {
        public TripMessage Create(TripMessage obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.TripMessages.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(TripMessage obj)
        {
            throw new NotImplementedException();
        }

        public ICollection<TripMessage> GetAll()
        {
            throw new NotImplementedException();
        }

        public TripMessage GetById(int id)
        {
            if (id < 0)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var tripMessage = context.TripMessages.Find(id);

                if (tripMessage != null)
                {
                    return tripMessage;
                }
                return null;
            }
        }

        public IEnumerable<TripMessage> GetByTripId(int tripId)
        {
            if (tripId < 0)
                return null;

            using (TripPlanner context = new TripPlanner())
            {
                var tripMessages = context.TripMessages
                                        .Where(x => x.TripId == tripId)
                                        .Join(context.Users, x => x.SenderId, y => y.Id, (a, b) => new TripMessage
                                        {
                                            Id = a.Id,
                                            SenderId = a.SenderId,
                                            TripId = a.TripId,
                                            SenderEmail = b.Email,
                                            Text = a.Text,
                                            Date = a.Date
                                        })
                                        .OrderByDescending(x => x.Date)
                                        .ToList();

                if (tripMessages != null)
                {
                    return tripMessages;
                }
                return Enumerable.Empty<TripMessage>();
            }
        }

        public TripMessage Update(TripMessage obj)
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