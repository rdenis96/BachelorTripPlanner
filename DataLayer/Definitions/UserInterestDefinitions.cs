using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Definitions
{
    static internal class UserInterestDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInterest>().HasKey(k => k.UserId);
            modelBuilder.Entity<UserInterest>().HasDiscriminator(x => x.Discriminator);
            modelBuilder.Entity<UserInterest>().Property(x => x.Discriminator).IsRequired(false);

            //  modelBuilder.Entity<UserInterestCountryAndCity>().HasBaseType<UserInterest>();
            modelBuilder.Entity<UserInterestCountryAndCity>().Property(x => x.Countries).IsRequired();
            modelBuilder.Entity<UserInterestCountryAndCity>().Property(x => x.Cities).IsRequired(false);

            //   modelBuilder.Entity<UserInterestWeather>().HasBaseType<UserInterest>();
            modelBuilder.Entity<UserInterestWeather>().Property(x => x.Weathers).IsRequired();

            // modelBuilder.Entity<UserInterestTouristAttraction>().HasBaseType<UserInterest>();
            modelBuilder.Entity<UserInterestTouristAttraction>().Property(x => x.TouristAttractions).IsRequired(false);

            //     modelBuilder.Entity<UserInterestTransport>().HasBaseType<UserInterest>();
            modelBuilder.Entity<UserInterestTransport>().Property(x => x.Transports).IsRequired();

            // modelBuilder.Entity<UserInterestExtended>().HasBaseType<UserInterest>();
        }
    }
}