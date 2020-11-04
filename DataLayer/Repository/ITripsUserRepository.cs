using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface ITripsUsersRepository : IBasicRepository<TripUser>
    {
        List<TripUser> GetByUserId(int userId);

        List<TripUser> GetByTripId(int tripId);

        TripUser GetByUserIdAndTripId(int userId, int tripId);

        bool IsUserAdmin(int userId, int tripId);
    }
}