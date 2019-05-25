using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}