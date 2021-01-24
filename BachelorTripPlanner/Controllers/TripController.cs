using BachelorTripPlanner.Attributes;
using BachelorTripPlanner.Models;
using BusinessLogic.Accounts;
using BusinessLogic.Interests;
using BusinessLogic.Notifications;
using BusinessLogic.Trips;
using DataLayer.CompositionRoot;
using Domain.Notifications;
using Domain.Notifications.Enums;
using Domain.Trips;
using Domain.Trips.Enums;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BachelorTripPlanner.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TripController : Controller
    {
        private readonly TripsWorker _tripsWorker;
        private readonly UserWorker _userWorker;
        private readonly TripsUsersWorker _tripsUsersWorker;
        private readonly UserInterestWorker _userInterestWorker;
        private readonly InterestsWorker _interestsWorker;
        private readonly TripMessagesWorker _tripMessagesWorker;
        private readonly NotificationsWorker _notificationsWorker;

        public TripController(ICompositionRoot compositionRoot)
        {
            _tripsWorker = compositionRoot.GetImplementation<TripsWorker>();
            _userWorker = compositionRoot.GetImplementation<UserWorker>();
            _userInterestWorker = compositionRoot.GetImplementation<UserInterestWorker>();
            _tripsUsersWorker = compositionRoot.GetImplementation<TripsUsersWorker>();
            _interestsWorker = compositionRoot.GetImplementation<InterestsWorker>();
            _tripMessagesWorker = compositionRoot.GetImplementation<TripMessagesWorker>();
            _notificationsWorker = compositionRoot.GetImplementation<NotificationsWorker>();
        }

        [HttpPost("[action]")]
        public IActionResult CreateTrip([ValidateUser] int userId, [FromBody] TripCreateModel tripCreate)
        {
            Trip trip;
            switch (tripCreate.TripType)
            {
                case TripType.Group:
                    trip = _tripsWorker.CreateTripForUsers(userId, tripCreate.TripName, tripCreate.TripType, tripCreate.InvitedPeople);
                    break;

                default:
                    trip = _tripsWorker.CreateTripForUser(userId, tripCreate.TripName, tripCreate.TripType);
                    break;
            }

            return Ok(trip);
        }

        [HttpGet("[action]")]
        public IActionResult GetUserHeaderTrips([ValidateUser] int userId)
        {
            var trips = _tripsUsersWorker.GetTripsForUser(userId, TripType.Group);
            List<HeaderUserTrips> userTrips = new List<HeaderUserTrips>();
            foreach (var trip in trips)
            {
                var userTripsCount = _tripsUsersWorker.GetTripUsersCount(trip.Id);
                var userTrip = new HeaderUserTrips
                {
                    TripId = trip.Id,
                    Name = trip.Name,
                    MembersCount = userTripsCount
                };
                userTrips.Add(userTrip);
            }
            return Ok(userTrips);
        }

        [HttpGet("[action]")]

        public IActionResult GetUserInterestForTrip([ValidateUser] int userId, int tripId)
        {
            var userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, tripId);
            if (userInterest == null)
            {
                return BadRequest("The user interests could not be retrieved!");
            }
            var userInterestView = new
            {
                countries = userInterest.Countries.ConvertStringToList(','),
                cities = userInterest.Cities.ConvertStringToList(','),
                weather = userInterest.Weather,
                touristAttractions = userInterest.TouristAttractions.ConvertStringToList('#'),
                transports = userInterest.Transports
            };
            return Ok(userInterestView);
        }

        [HttpGet("[action]")]
        public IActionResult GetSuggestedInterests(int tripId)
        {
            var tripInterests = _interestsWorker.GetSuggestedInterestsByTrip(tripId);

            var result = new List<InterestsModel>();
            foreach (var interest in tripInterests)
            {
                result.Add(new InterestsModel
                {
                    Country = interest.Country,
                    City = interest.City,
                    Weather = interest.Weather.ConvertStringToList(','),
                    TouristAttractions = interest.TouristAttractions.ConvertStringToList('#').Where(x => string.IsNullOrWhiteSpace(x) == false).ToList(),
                    Transports = interest.Transport.ConvertStringToList(','),
                    LinkImage = interest.LinkImage,
                    LinkWikipediaCity = interest.LinkWikipediaCity
                });
            }

            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetMessages(int tripId)
        {
            var tripMessages = _tripMessagesWorker.GetByTripId(tripId);

            return Ok(tripMessages);
        }

        [HttpPost("[action]")]
        public IActionResult CreateMessage([FromBody] TripMessage newMessage)
        {
            var tripMessages = _tripMessagesWorker.Create(newMessage);

            return Ok(tripMessages);
        }

        [HttpGet("[action]")]
        public IActionResult IsUserAdmin([ValidateUser] int userId, int tripId)
        {
            var isAdmin = _tripsUsersWorker.IsUserAdmin(userId, tripId);
            return Ok(
                new { isAdmin });
        }

        [HttpGet("[action]")]
        public IActionResult GetTripUsers(int tripId)
        {
            var result = _tripsUsersWorker.GetTripUsers(tripId);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateTripUsers([FromBody] List<TripUser> tripUsers)
        {
            var result = _tripsUsersWorker.UpdateMany(tripUsers);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult AddNewTripMember([ValidateUser] int adminId, int tripId, string newMemberEmail)
        {
            var result = _tripsWorker.AddNewTripMember(adminId, tripId, newMemberEmail);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult RemoveUserFromTrip([ValidateUser] int adminId, [ValidateUser] int userId, int tripId)
        {
            var result = _tripsWorker.RemoveUserFromTrip(userId, tripId);
            if (result)
            {
                _notificationsWorker.Create(new Notification
                {
                    SenderId = adminId,
                    TripId = tripId,
                    Type = NotificationType.TripKicked,
                    UserId = userId,
                    Date = DateTime.UtcNow
                });
            }
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult ResetUserInterests([ValidateUser] int userId, int tripid)
        {
            var result = _tripsWorker.ResetUserInterests(userId, tripid);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult LeaveTrip([ValidateUser] int userId, int tripid)
        {
            try
            {
                _tripsWorker.LeaveTrip(userId, tripid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetTrip(int tripId)
        {
            try
            {
                var result = _tripsWorker.Get(tripId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}