using Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class NotificationDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>().HasKey(k => k.Id);
            modelBuilder.Entity<Notification>().Property(x => x.Type).IsRequired();
            modelBuilder.Entity<Notification>().Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<Notification>().Property(x => x.SenderId).IsRequired(false);
            modelBuilder.Entity<Notification>().Property(x => x.TripId).IsRequired(false);
            modelBuilder.Entity<Notification>().Property(x => x.Date).IsRequired();
            modelBuilder.Entity<Notification>().Ignore(x => x.SenderEmail);
            modelBuilder.Entity<Notification>().Ignore(x => x.TripName);
        }
    }
}