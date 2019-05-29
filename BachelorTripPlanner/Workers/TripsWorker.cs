using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Repository;
using DataLayer.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Workers
{
    public class TripsWorker
    {
        private IUserRepository _userRepository;
        private ITripsRepository _tripsRepository;
        private ITripsUsersRepository _tripsUsersRepository;

        public TripsWorker()
        {
            _userRepository = new UserRepository();
            _tripsRepository = new TripsRepository();
            _tripsUsersRepository = new TripsUsersRepository();
        }

        public Trip CreateTripForUser(int userId, string tripName, TripType type)
        {
            var trip = new Trip
            {
                Name = tripName,
                Interests = new UserInterestWrapper(),
                Type = type
            };
            var createdTrip = _tripsRepository.Create(trip);
            var userTrip = new TripUser
            {
                HasAcceptedInvitation = true,
                IsGroupAdmin = true,
                Interests = new UserInterestWrapper(),
                UserId = userId,
                TripId = createdTrip.Id
            };
            var createdUserTrip = _tripsUsersRepository.Create(userTrip);
            if (userTrip != null && createdUserTrip != null)
            {
                return createdTrip;
            }
            return null;
        }

        public Trip CreateTripForUsers(int userId, string tripName, TripType type, List<string> invitedPeopleEmails)
        {
            var usersIdByEmail = _userRepository.GetIdsByEmail(invitedPeopleEmails);
            var createdTrip = CreateTripForUser(userId, tripName, type);

            foreach (var id in usersIdByEmail)
            {
                var userTrip = new TripUser
                {
                    HasAcceptedInvitation = false,
                    IsGroupAdmin = false,
                    Interests = new UserInterestWrapper(),
                    UserId = id,
                    TripId = createdTrip.Id
                };
                var createdUserTrip = _tripsUsersRepository.Create(userTrip);
            }

            if (createdTrip != null)
            {
                return createdTrip;
            }
            return null;
        }
    }
}