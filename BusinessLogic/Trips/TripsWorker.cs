using Domain.Interests;
using Domain.Notifications;
using Domain.Notifications.Enums;
using Domain.Repository;
using Domain.Trips;
using Domain.Trips.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Trips
{
    public class TripsWorker
    {
        private IUserRepository _userRepository;
        private ITripsRepository _tripsRepository;
        private ITripsUsersRepository _tripsUsersRepository;
        private IUserInterestRepository _userInterestRepository;
        private INotificationsRepository _notificationsRepository;

        public TripsWorker(IUserRepository userRepository,
            ITripsRepository tripsRepository,
            ITripsUsersRepository tripsUsersRepository,
            IUserInterestRepository userInterestRepository,
            INotificationsRepository notificationsRepository)
        {
            _userRepository = userRepository;
            _tripsRepository = tripsRepository;
            _tripsUsersRepository = tripsUsersRepository;
            _userInterestRepository = userInterestRepository;
            _notificationsRepository = notificationsRepository;
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

        public Trip Update(Trip trip)
        {
            var result = _tripsRepository.Update(trip);
            return result;
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

        public bool RemoveUserFromTrip(int userId, int tripId)
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

        public void LeaveTrip(int userId, int tripId)
        {
            var tripUser = _tripsUsersRepository.GetByUserIdAndTripId(userId, tripId);
            if (tripUser == null)
            {
                throw new Exception($"The user {userId} was not found for trip {tripId}");
            }

            var tripAdminsCount = _tripsUsersRepository.CountAdmins(tripId);
            var tripUsersCount = _tripsUsersRepository.CountTripUsers(tripId);

            var trip = _tripsRepository.GetById(tripId);
            if (trip != null && trip.Type == TripType.Group && tripAdminsCount == 1 && tripUsersCount > 1)
            {
                throw new Exception("You can't leave the trip because there is no other admin!");
            }

            RemoveUserFromTrip(userId, tripId);
            if (tripAdminsCount <= 1 && tripUsersCount <= 1)
            {
                _tripsRepository.Delete(trip);
            }
        }

        public Trip Get(int tripId)
        {
            var result = _tripsRepository.GetById(tripId);
            return result;
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

            var userInterest = GenerateUserInterestForTrip(userId, createdUserTrip.TripId.GetValueOrDefault());
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