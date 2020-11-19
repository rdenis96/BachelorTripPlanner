using DataLayer.Context;
using Domain.Notifications;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Notifications
{
    public class NotificationsRepository : INotificationsRepository
    {
        public Notification Create(Notification obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.Notifications.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(Notification obj)
        {
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                var notification = context.Notifications.Find(obj.Id);
                if (notification != null)
                {
                    context.Entry(notification).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();
                }
                return true;
            }
        }

        public ICollection<Notification> GetAll()
        {
            throw new NotImplementedException();
        }

        public Notification GetById(int id)
        {
            if (id < 0)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var notification = context.Notifications.Find(id);

                if (notification != null)
                {
                    return notification;
                }
                return null;
            }
        }

        public IEnumerable<Notification> GetByUserId(int userId)
        {
            if (userId < 0)
                return null;

            using (TripPlanner context = new TripPlanner())
            {
                var tripMessages = context.Notifications
                                        .Where(x => x.UserId == userId)
                                        .AsEnumerable()
                                        .Join(context.Users, x => x.SenderId, y => y.Id, (a, b) => new Notification
                                        {
                                            Id = a.Id,
                                            Type = a.Type,
                                            UserId = a.UserId,
                                            SenderId = a.SenderId,
                                            TripId = a.TripId,
                                            SenderEmail = b.Email
                                        })
                                        .Join(context.Trips, x => x.TripId, y => y.Id, (a, b) => new Notification
                                        {
                                            Id = a.Id,
                                            Type = a.Type,
                                            UserId = a.UserId,
                                            SenderId = a.SenderId,
                                            TripId = a.TripId,
                                            SenderEmail = a.SenderEmail,
                                            TripName = b.Name
                                        })
                                        .OrderByDescending(x => x.Date)
                                        .Take(5)
                                        .ToList();

                if (tripMessages != null)
                {
                    return tripMessages;
                }
                return Enumerable.Empty<Notification>();
            }
        }

        public Notification Update(Notification obj)
        {
            throw new NotImplementedException();
        }
    }
}