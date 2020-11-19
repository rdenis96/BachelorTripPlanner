using Domain.Repository;
using Domain.Trips;
using Domain.Trips.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Trips
{
    public class TripsUsersWorker
    {
        private readonly ITripsRepository _tripsRepository;
        private readonly ITripsUsersRepository _tripsUsersRepository;

        public TripsUsersWorker(ITripsRepository tripsRepository, ITripsUsersRepository tripsUsersRepository)
        {
            _tripsRepository = tripsRepository;
            _tripsUsersRepository = tripsUsersRepository;
        }

        public List<Trip> GetTripsForUser(int userId, TripType? type = null, bool includeDeleted = false)
        {
            var tripsForUser = _tripsUsersRepository.GetByUserId(userId, includeDeleted: includeDeleted);
            List<Trip> trips = new List<Trip>();
            foreach (var tripUser in tripsForUser)
            {
                var trip = _tripsRepository.GetById(tripUser.TripId.GetValueOrDefault());
                if (trip != null)
                {
                    trips.Add(trip);
                }
            }
            if (type.HasValue)
            {
                trips = trips.Where(x => x.Type == type).ToList();
            }

            trips = trips.OrderByDescending(x => x.Id).ToList();
            return trips;
        }

        public int GetTripUsersCount(int tripId)
        {
            var tripUsersCount = _tripsUsersRepository.GetLazyByTripId(tripId)?.Where(x => x.HasAcceptedInvitation)?.Count();
            return tripUsersCount.GetValueOrDefault();
        }

        public bool IsUserAdmin(int userId, int tripId)
        {
            var result = _tripsUsersRepository.IsUserAdmin(userId, tripId);
            return result;
        }

        public IEnumerable<TripUser> GetTripUsers(int tripId)
        {
            var result = _tripsUsersRepository.GetByTripId(tripId);
            return result;
        }

        public IEnumerable<TripUser> UpdateMany(IEnumerable<TripUser> tripUsers)
        {
            var result = _tripsUsersRepository.UpdateMany(tripUsers);
            return result;
        }
    }
}