using Domain.Trips;
using Domain.Trips.Enums;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface ITripsRepository : IBasicRepository<Trip>
    {
        List<Trip> GetByType(TripType type);
    }
}