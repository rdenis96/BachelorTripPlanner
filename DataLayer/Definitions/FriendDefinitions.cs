using Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Definitions
{
    static internal class FriendDefinitions
    {
        public static void Set(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>().HasKey(x => x.Id);
            modelBuilder.Entity<Friend>().Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<Friend>().Property(x => x.FriendId).IsRequired();
            modelBuilder.Entity<Friend>().Property(x => x.IsDeleted).IsRequired();
            modelBuilder.Entity<Friend>().HasOne(c => c.FriendAccount).WithMany().HasForeignKey(c => c.FriendId);
        }
    }
}