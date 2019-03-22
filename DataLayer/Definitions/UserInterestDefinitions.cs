using DataLayer.Enums;
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
            modelBuilder.Entity<UserInterest>().Property(x => x.Countries).IsRequired();
            modelBuilder.Entity<UserInterest>().Property(x => x.Cities).IsRequired(false);
            modelBuilder.Entity<UserInterest>().Property(x => x.Weathers).IsRequired().HasDefaultValue(WeathersEnum.None);
            modelBuilder.Entity<UserInterest>().Property(x => x.TouristAttractions).IsRequired(false);
            modelBuilder.Entity<UserInterest>().Property(x => x.Transports).IsRequired().HasDefaultValue(TransportsEnum.None);
        }
    }
}