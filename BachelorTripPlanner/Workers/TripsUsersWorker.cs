using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Repository;
using DataLayer.Repository.Implementation;
using System.Collections.Generic;
using System.Linq;

namespace BachelorTripPlanner.Workers
{
    public class TripsUsersWorker
    {
        private readonly ITripsRepository _tripsRepository;
        private readonly ITripsUsersRepository _tripsUsersRepository;

        public TripsUsersWorker()
        {
            _tripsRepository = new TripsRepository();
            _tripsUsersRepository = new TripsUsersRepository();
        }

        public List<Trip> GetTripsForUser(int userId, TripType? type = null)
        {
            var tripsForUser = _tripsUsersRepository.GetByUserId(userId);
            List<Trip> trips = new List<Trip>();
            foreach (var tripUser in tripsForUser)
            {
                var trip = _tripsRepository.GetById(tripUser.TripId);
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