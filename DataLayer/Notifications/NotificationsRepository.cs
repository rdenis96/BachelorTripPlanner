using DataLayer.Context;
using Domain.Common.Constants;
using Domain.Notifications;
using Domain.Notifications.Enums;
using Domain.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

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
            List<Notification> notifications = new List<Notification>();

            using (var connection = new SqlConnection(GlobalConstants.SqlDatabaseConnection))
            {
                connection.Open();
                using (SqlCommand command = GetNotificationByUserIdQuery(connection, userId))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            return notifications;
                        }

                        var ordinalId = reader.GetOrdinal(nameof(Notification.Id));
                        var ordinalType = reader.GetOrdinal(nameof(Notification.Type));
                        var ordinalUserId = reader.GetOrdinal(nameof(Notification.UserId));
                        var ordinalSenderId = reader.GetOrdinal(nameof(Notification.SenderId));
                        var ordinalTripId = reader.GetOrdinal(nameof(Notification.TripId));
                        var ordinalSenderEmail = reader.GetOrdinal(nameof(Notification.SenderEmail));
                        var ordinalTripName = reader.GetOrdinal(nameof(Notification.TripName));

                        while (reader.Read())
                        {
                            Notification notification = new Notification();

                            notification.Id = reader.IsDBNull(ordinalId) ? 0 : reader.GetInt32(ordinalId);
                            notification.Type = reader.IsDBNull(ordinalType) ? 0 : (NotificationType)reader.GetInt32(ordinalType);
                            notification.UserId = reader.IsDBNull(ordinalUserId) ? 0 : reader.GetInt32(ordinalUserId);
                            notification.SenderId = reader.IsDBNull(ordinalSenderId) ? 0 : reader.GetInt32(ordinalSenderId);
                            notification.TripId = reader.IsDBNull(ordinalTripId) ? 0 : reader.GetInt32(ordinalTripId);
                            notification.SenderEmail = reader.IsDBNull(ordinalSenderEmail) ? string.Empty : reader.GetString(ordinalSenderEmail);
                            notification.TripName = reader.IsDBNull(ordinalTripName) ? string.Empty : reader.GetString(ordinalTripName);

                            notifications.Add(notification);
                        }
                    }
                }
                return notifications;
            }
        }

        //public IEnumerable<Notification> GetByUserId(int userId)
        //{
        //    if (userId < 0)
        //        return null;

        //    using (TripPlanner context = new TripPlanner())
        //    {
        //        var tripMessages = context.Notifications
        //                                .Where(x => x.UserId == userId)
        //                                .AsEnumerable()
        //                                .Join(context.Users, x => x.UserId, y => y.Id, (a, b) => new Notification
        //                                {
        //                                    Id = a.Id,
        //                                    Type = a.Type,
        //                                    UserId = a.UserId,
        //                                    SenderId = a.SenderId,
        //                                    TripId = a.TripId,
        //                                    SenderEmail = b.Email
        //                                })
        //                                .Join(context.Trips, x => x.TripId, y => y.Id, (a, b) => new Notification
        //                                {
        //                                    Id = a.Id,
        //                                    Type = a.Type,
        //                                    UserId = a.UserId,
        //                                    SenderId = a.SenderId,
        //                                    TripId = a.TripId,
        //                                    SenderEmail = a.SenderEmail,
        //                                    TripName = b.Name
        //                                })
        //                                .OrderByDescending(x => x.Date)
        //                                .Take(5)
        //                                .ToList();

        //        if (tripMessages != null)
        //        {
        //            return tripMessages;
        //        }
        //        return Enumerable.Empty<Notification>();
        //    }
        //}

        public Notification Update(Notification obj)
        {
            throw new NotImplementedException();
        }

        private SqlCommand GetNotificationByUserIdQuery(SqlConnection connection, int userId)
        {
            SqlCommand command = connection.CreateCommand();
            string query = @"SELECT N.*,
                                    U.Email as 'SenderEmail',
                                    T.Name as 'TripName'
                              FROM Notifications N
                              JOIN Users U
                              ON U.Id = N.SenderId
                              LEFT JOIN Trips T
                              ON T.Id = N.TripId
                              WHERE N.UserId = @userId
                              ORDER by N.Date DESC";

            command.CommandText = query;
            command.Parameters.AddWithValue("@userId", userId);

            return command;
        }
    }
}