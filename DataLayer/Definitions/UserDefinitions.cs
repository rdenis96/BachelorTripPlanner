using Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class UserDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(k => k.Id);
            modelBuilder.Entity<User>().Property(p => p.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().Property(p => p.Password).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.RegisterDate).IsRequired().HasColumnType("datetime2");
            modelBuilder.Entity<User>().Property(p => p.LastOnline).IsRequired(false).HasColumnType("datetime2");
            modelBuilder.Entity<User>().Property(p => p.Ip).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Phone).IsRequired(false);
        }
    }
}