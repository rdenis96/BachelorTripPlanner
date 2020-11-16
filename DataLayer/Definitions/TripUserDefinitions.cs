using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class TripUserDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripUser>().HasKey(x => x.Id);
            modelBuilder.Entity<TripUser>().Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<TripUser>().Property(x => x.TripId).IsRequired();
            modelBuilder.Entity<TripUser>().Property(x => x.HasAcceptedInvitation).IsRequired();
            modelBuilder.Entity<TripUser>().Property(x => x.IsGroupAdmin).IsRequired();
            modelBuilder.Entity<TripUser>().Property(x => x.IsDeleted).IsRequired();
            modelBuilder.Entity<TripUser>().HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
        }
    }
}