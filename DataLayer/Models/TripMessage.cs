using System;

namespace DataLayer.Models
{
    public class TripMessage : IEquatable<TripMessage>
    {
        public int Id { get; set; }

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
                   Id == other.Id &&
                   SenderId == other.SenderId &&
                   TripId == other.TripId &&
                   Text == other.Text &&
                   Date == other.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, SenderId, TripId, Text, Date);
        }
    }
}