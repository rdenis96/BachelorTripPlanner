using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repository
{
    public interface ITripsUsersRepository : IBasicRepository<TripUser>
    {
        List<TripUser> GetByUserId(int userId);

        List<TripUser> GetByTripId(int tripId);

        TripUser GetByUserIdAndTripId(int userId, int tripId);
    }
}