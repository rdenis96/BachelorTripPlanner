using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface IUserInterestRepository : IBasicRepository<UserInterest>
    {
        UserInterest GetByUserId(int userId);

        UserInterest GetByUserIdAndTripId(int userId, int tripId);

        IEnumerable<UserInterest> GetByTripId(int tripId);
    }
}