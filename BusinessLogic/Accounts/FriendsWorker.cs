using Domain.Accounts;
using Domain.Repository;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Accounts
{
    public class FriendsWorker
    {
        private IFriendsRepository _friendsRepository;

        public FriendsWorker(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        public Friend Create(int userId, int friendId)
        {
            var friend = _friendsRepository.GetByUserIdAndFriendId(userId, friendId);
            if (friend == null)
            {
                friend = GenerateFriend(userId, friendId);
                friend = _friendsRepository.Create(friend);
            }
            else if (friend.IsDeleted)
            {
                friend.IsDeleted = false;
                friend.CreatedDate = DateTime.Now;
                friend = _friendsRepository.Update(friend);
            }

            return friend;
        }

        public ICollection<Friend> GetByUserId(int userId)
        {
            var friends = _friendsRepository.GetByUserId(userId);
            return friends;
        }

        public bool Remove(int id)
        {
            var friend = _friendsRepository.GetById(id);
            var isDeleted = _friendsRepository.Delete(friend);
            return isDeleted;
        }

        private Friend GenerateFriend(int userId, int friendId)
        {
            return new Friend
            {
                UserId = userId,
                FriendId = friendId,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };
        }
    }
}