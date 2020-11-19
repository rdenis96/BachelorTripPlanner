using Domain.Common;
using System;

namespace Domain.Trips
{
    public class TripMessage : BaseEntity, IEquatable<TripMessage>
    {
        public int SenderId { get; set; }

        public int TripId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string SenderEmail { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TripMessage);
        }

        public bool Equals(TripMessage other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   SenderId == other.SenderId &&
                   TripId == other.TripId &&
                   Text == other.Text &&
                   Date == other.Date &&
                   SenderEmail == other.SenderEmail;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, SenderId, TripId, Text, Date, SenderEmail);
        }
    }
}