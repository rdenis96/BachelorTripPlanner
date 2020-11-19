using Domain.Trips;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface ITripsUsersRepository : IBasicRepository<TripUser>
    {
        List<TripUser> GetByUserId(int userId, bool includeDeleted = false);

        List<TripUser> GetLazyByTripId(int tripId);

        List<TripUser> GetByTripId(int tripId);

        TripUser GetByUserIdAndTripId(int userId, int tripId);

        bool IsUserAdmin(int userId, int tripId);

        IEnumerable<TripUser> UpdateMany(IEnumerable<TripUser> tripUsers);

        int CountAdmins(int tripId);

        int CountTripUsers(int tripId);
    }
}