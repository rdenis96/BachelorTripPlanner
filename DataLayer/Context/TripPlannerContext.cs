using DataLayer.Constants;
using DataLayer.Definitions;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Context
{
    public class TripPlanner : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<Interest> Interests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GlobalConstants.SqlDatabaseConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserDefinitions.Set(modelBuilder);
            UserInterestDefinitions.Set(modelBuilder);
            InterestDefinitions.Set(modelBuilder);
        }
    }
}