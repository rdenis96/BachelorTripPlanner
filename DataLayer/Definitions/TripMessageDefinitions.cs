using Domain.Trips;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class TripMessageDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripMessage>().HasKey(k => k.Id);
            modelBuilder.Entity<TripMessage>().Property(x => x.SenderId).IsRequired();
            modelBuilder.Entity<TripMessage>().Property(x => x.TripId).IsRequired();
            modelBuilder.Entity<TripMessage>().Property(x => x.Text).IsRequired();
            modelBuilder.Entity<TripMessage>().Property(x => x.Date).IsRequired().HasColumnType("datetime2");
            modelBuilder.Entity<TripMessage>().Ignore(x => x.SenderEmail);
        }
    }
}