using Domain.Interests;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IUserInterestRepository : IBasicRepository<UserInterest>
    {
        UserInterest GetByUserId(int userId);

        UserInterest GetByUserIdAndTripId(int userId, int tripId);

        IEnumerable<UserInterest> GetByTripId(int tripId);
    }
}