using Domain.Trips;
using System.Collections.Generic;

namespace Domain.Repository
{
    public interface ITripMessagesRepository : IBasicRepository<TripMessage>
    {
        IEnumerable<TripMessage> GetByTripId(int tripId);
    }
}