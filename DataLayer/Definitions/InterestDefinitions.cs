using Domain.Interests;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class InterestDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interest>().HasKey(k => k.Id);
            modelBuilder.Entity<Interest>().Property(p => p.Country).IsRequired();
            modelBuilder.Entity<Interest>().Property(p => p.City).IsRequired();
            modelBuilder.Entity<Interest>().Property(p => p.GeneralWeather).IsRequired();
            modelBuilder.Entity<Interest>().Property(p => p.Weather).IsRequired();
            modelBuilder.Entity<Interest>().Property(p => p.TouristAttractions).IsRequired();
            modelBuilder.Entity<Interest>().Property(p => p.Transport).IsRequired();
            modelBuilder.Entity<Interest>().Property(p => p.LinkImage).IsRequired(false);
            modelBuilder.Entity<Interest>().Property(p => p.LinkWikipediaCity).IsRequired(false);

            //map many-to-many relationship for similar interests
            modelBuilder.Entity<SimilarInterest>().HasKey(k => k.Id);
            modelBuilder.Entity<SimilarInterest>().Property(p => p.InterestId).IsRequired();
            modelBuilder.Entity<SimilarInterest>().Property(p => p.SimInterestId).IsRequired();

            modelBuilder.Entity<SimilarInterest>().HasOne(t => t.Interest).WithMany(x => x.SimilarInterests).HasForeignKey(f => f.InterestId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}