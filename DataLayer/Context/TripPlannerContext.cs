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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=WORKSTATION9\\SQLEXPRESS;Initial Catalog=TripPlanner;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserDefinitions.Set(modelBuilder);
            UserInterestDefinitions.Set(modelBuilder);
        }
    }
}