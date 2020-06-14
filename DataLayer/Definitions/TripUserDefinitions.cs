using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

            modelBuilder.Entity<TripUser>().OwnsOne(x => x.Interests,
                g =>
                {
                    g.Property(x => x.Countries);
                    g.Property(x => x.Cities);
                    g.Property(x => x.Weather);
                    g.Property(x => x.TouristAttractions);
                    g.Property(x => x.Transports);
                });
        }
    }
}