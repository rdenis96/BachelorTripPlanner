using Domain.Common;
using Domain.Notifications.Enums;
using System;

namespace Domain.Notifications
{
    public class Notification : UserTripBase, IEquatable<Notification>
    {
        public NotificationType Type { get; set; }

        public int? SenderId { get; set; }

        public DateTime Date { get; set; }

        public string SenderEmail { get; set; }

        public string TripName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Notification);
        }

        public bool Equals(Notification other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   UserId == other.UserId &&
                   TripId == other.TripId &&
                   Type == other.Type &&
                   SenderId == other.SenderId &&
                   Date == other.Date &&
                   SenderEmail == other.SenderEmail &&
                   TripName == other.TripName;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Id);
            hash.Add(UserId);
            hash.Add(TripId);
            hash.Add(Type);
            hash.Add(SenderId);
            hash.Add(Date);
            hash.Add(SenderEmail);
            hash.Add(TripName);
            return hash.ToHashCode();
        }
    }
}