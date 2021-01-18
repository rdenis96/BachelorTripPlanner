using DataLayer.Context;
using Domain.Accounts;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Accounts
{
    public class FriendsRepository : IFriendsRepository
    {
        public Friend Create(Friend obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.Friends.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(Friend obj)
        {
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                var friend = context.Friends.Find(obj.Id);
                if (friend != null)
                {
                    var receiverFriendship = context.Friends.Where(x => x.UserId == friend.FriendId && x.FriendId == friend.UserId).FirstOrDefault();

                    friend.IsDeleted = true;
                    receiverFriendship.IsDeleted = true;

                    context.SaveChanges();
                }
                return true;
            }
        }

        public ICollection<Friend> GetAll()
        {
            throw new NotImplementedException();
        }

        public Friend GetById(int id)
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Friends.Find(id);
            }
        }

        public ICollection<Friend> GetByUserId(int userId, bool includeDeleted = false)
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Friends.Where(c => c.UserId == userId && c.IsDeleted == includeDeleted).Include(x => x.FriendAccount).ToList();
            }
        }

        public Friend GetByUserIdAndFriendId(int userId, int friendId)
        {
            using (TripPlanner context = new TripPlanner())
            {
                return context.Friends.Where(c => c.UserId == userId && c.FriendId == friendId).FirstOrDefault();
            }
        }

        public Friend Update(Friend obj)
        {
            if (obj == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.SaveChanges();

                var result = context.Friends.Find(obj.Id);
                return result;
            }
        }
    }
}