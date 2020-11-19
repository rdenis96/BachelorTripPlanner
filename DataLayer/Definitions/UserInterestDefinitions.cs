using Domain.Interests;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class UserInterestDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInterest>().HasKey(k => k.Id);
            modelBuilder.Entity<UserInterest>().Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<UserInterest>().Property(x => x.TripId).IsRequired(false);
            modelBuilder.Entity<UserInterest>().Property(x => x.Countries).IsRequired();
            modelBuilder.Entity<UserInterest>().Property(x => x.Cities).IsRequired(false);
            modelBuilder.Entity<UserInterest>().Property(x => x.Weather).IsRequired();
            modelBuilder.Entity<UserInterest>().Property(x => x.TouristAttractions).IsRequired(false);
            modelBuilder.Entity<UserInterest>().Property(x => x.Transports).IsRequired();
        }
    }
}