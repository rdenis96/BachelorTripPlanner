using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Definitions
{
    static internal class TripDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().HasKey(k => k.Id);

            modelBuilder.Entity<Trip>().OwnsOne(x => x.Interests,
                g =>
                {
                    g.Property(x => x.Countries);
                    g.Property(x => x.Cities);
                    g.Property(x => x.Weather);
                    g.Property(x => x.TouristAttractions);
                    g.Property(x => x.Transports);
                });

            modelBuilder.Entity<Trip>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Trip>().Property(x => x.Type).IsRequired();
        }
    }
}