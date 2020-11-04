using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface ITripMessagesRepository : IBasicRepository<TripMessage>
    {
        IEnumerable<TripMessage> GetByTripId(int tripId);
    }
}