using Domain.Accounts;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IFriendsRepository : IBasicRepository<Friend>
    {
        ICollection<Friend> GetByUserId(int userId, bool includeDeleted = false);

        Friend GetByUserIdAndFriendId(int userId, int friendId);
    }
}