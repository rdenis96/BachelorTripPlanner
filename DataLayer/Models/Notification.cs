using DataLayer.Enums;
using System;

namespace DataLayer.Models
{
    public class Notification : IEquatable<Notification>
    {
        public int Id { get; set; }

        public NotificationType Type { get; set; }

        public int UserId { get; set; }

        public int? SenderId { get; set; }

        public int? TripId { get; set; }

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
                   Id == other.Id &&
                   Type == other.Type &&
                   UserId == other.UserId &&
                   SenderId == other.SenderId &&
                   TripId == other.TripId &&
                   Date == other.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type, UserId, SenderId, TripId, Date);
        }
    }
}