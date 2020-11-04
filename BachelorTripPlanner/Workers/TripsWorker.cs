using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Repository;
using DataLayer.Repository.Implementation;
using System;
using System.Collections.Generic;

namespace BachelorTripPlanner.Workers
{
    public class TripsWorker
    {
        private IUserRepository _userRepository;
        private ITripsRepository _tripsRepository;
        private ITripsUsersRepository _tripsUsersRepository;
        private IUserInterestRepository _userInterestRepository;
        private INotificationsRepository _notificationsRepository;

        public TripsWorker()
        {
            _userRepository = new UserRepository();
            _tripsRepository = new TripsRepository();
            _tripsUsersRepository = new TripsUsersRepository();
            _userInterestRepository = new UserInterestRepository();
            _notificationsRepository = new NotificationsRepository();
        }

        public Trip CreateTripForUser(int userId, string tripName, TripType type)
        {
            var trip = new Trip
            {
                Name = tripName,
                Type = type
            };
            var createdTrip = _tripsRepository.Create(trip);
            var userTrip = new TripUser
            {
                HasAcceptedInvitation = true,
                IsGroupAdmin = true,
                UserId = userId,
                TripId = createdTrip.Id
            };
            var createdUserTrip = _tripsUsersRepository.Create(userTrip);
            var userInterest = GenerateUserInterestForTrip(userId, createdTrip.Id);
            userInterest = _userInterestRepository.Create(userInterest);

            if (userTrip != null && createdUserTrip != null && userInterest != null)
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
                    UserId = id,
                    TripId = createdTrip.Id
                };
                var createdUserTrip = _tripsUsersRepository.Create(userTrip);

                var userInterest = GenerateUserInterestForTrip(id, createdUserTrip.TripId);
                userInterest = _userInterestRepository.Create(userInterest);
                if (userInterest != null)
                {
                    _notificationsRepository.Create(new Notification
                    {
                        SenderId = userId,
                        TripId = createdTrip.Id,
                        Type = NotificationType.TripInvitation,
                        UserId = id,
                        Date = DateTime.UtcNow
                    });
                }
            }

            if (createdTrip != null)
            {
                return createdTrip;
            }
            return null;
        }

        private UserInterest GenerateUserInterestForTrip(int userId, int tripId)
        {
            var userInterest = new UserInterest
            {
                TripId = tripId,
                UserId = userId,
                Cities = string.Empty,
                Countries = string.Empty,
                TouristAttractions = string.Empty,
                Transports = string.Empty,
                Weather = string.Empty
            };
            return userInterest;
        }
    }
}