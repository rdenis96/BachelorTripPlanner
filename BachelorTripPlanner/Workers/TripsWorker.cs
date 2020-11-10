using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Repository;
using DataLayer.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;

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
                AddTripMember(userId, createdTrip.Id, id);
            }

            if (createdTrip != null)
            {
                return createdTrip;
            }
            return null;
        }

        public IEnumerable<TripUser> AddNewTripMember(int adminId, int tripId, string newMemberEmail)
        {
            var userIdByEmail = _userRepository.GetIdsByEmail(new List<string> { newMemberEmail }).FirstOrDefault();

            if (userIdByEmail > 0)
            {
                AddTripMember(adminId, tripId, userIdByEmail);
            }

            var tripUsers = _tripsUsersRepository.GetByTripId(tripId);
            return tripUsers;
        }

        public bool ResetUserInterests(int userId, int tripId)
        {
            var userInterests = _userInterestRepository.GetByUserIdAndTripId(userId, tripId);
            if (userInterests != null)
            {
                userInterests.Cities = string.Empty;
                userInterests.Countries = string.Empty;
                userInterests.TouristAttractions = string.Empty;
                userInterests.Transports = string.Empty;
                userInterests.Weather = string.Empty;

                var userInterestsUpdated = _userInterestRepository.Update(userInterests);
                var result = userInterests.Equals(userInterestsUpdated);
                return result;
            }

            return false;
        }

        public bool RemoveUserFromTrip(int adminId, int userId, int tripId)
        {
            var userInterest = _userInterestRepository.GetByUserIdAndTripId(userId, tripId);
            var isDeleted = _userInterestRepository.Delete(userInterest);
            if (isDeleted)
            {
                var tripUser = _tripsUsersRepository.GetByUserIdAndTripId(userId, tripId);
                isDeleted = _tripsUsersRepository.Delete(tripUser);
                if (isDeleted)
                {
                    return isDeleted;
                }
            }
            return false;
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

        private void AddTripMember(int adminId, int tripid, int userId)
        {
            var userTrip = new TripUser
            {
                HasAcceptedInvitation = false,
                IsGroupAdmin = false,
                UserId = userId,
                TripId = tripid
            };
            var createdUserTrip = _tripsUsersRepository.Create(userTrip);

            var userInterest = GenerateUserInterestForTrip(userId, createdUserTrip.TripId);
            userInterest = _userInterestRepository.Create(userInterest);
            if (userInterest != null)
            {
                _notificationsRepository.Create(new Notification
                {
                    SenderId = adminId,
                    TripId = tripid,
                    Type = NotificationType.TripInvitation,
                    UserId = userId,
                    Date = DateTime.UtcNow
                });
            }
        }
    }
}