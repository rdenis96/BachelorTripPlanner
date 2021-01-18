using DataLayer.Definitions;
using Domain.Accounts;
using Domain.Common.Constants;
using Domain.Interests;
using Domain.Notifications;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Context
{
    public class TripPlanner : DbContext
    {
        public TripPlanner() : base()
        {
        }

        public TripPlanner(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripUser> TripUsers { get; set; }
        public DbSet<TripMessage> TripMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GlobalConstants.SqlDatabaseConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserDefinitions.Set(modelBuilder);
            UserInterestDefinitions.Set(modelBuilder);
            InterestDefinitions.Set(modelBuilder);
            TripDefinitions.Set(modelBuilder);
            TripUserDefinitions.Set(modelBuilder);
            TripMessageDefinitions.Set(modelBuilder);
            NotificationDefinitions.Set(modelBuilder);
            FriendDefinitions.Set(modelBuilder);
        }
    }
}