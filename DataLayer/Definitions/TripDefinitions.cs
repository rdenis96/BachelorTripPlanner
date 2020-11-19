using Domain.Trips;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class TripDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().HasKey(k => k.Id);
            modelBuilder.Entity<Trip>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Trip>().Property(x => x.Type).IsRequired();
            modelBuilder.Entity<Trip>().Property(x => x.IsDeleted).IsRequired();
        }
    }
}