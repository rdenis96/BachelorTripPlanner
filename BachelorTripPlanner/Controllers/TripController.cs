using BachelorTripPlanner.Models;
using BachelorTripPlanner.Workers;
using DataLayer.Enums;
using DataLayer.Helpers;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace BachelorTripPlanner.Controllers
{
    [Route("api/[controller]")]
    public class TripController : Controller
    {
        private readonly TripsWorker _tripsWorker;
        private readonly UserWorker _userWorker;
        private readonly TripsUsersWorker _tripsUsersWorker;
        private readonly UserInterestWorker _userInterestWorker;
        private readonly InterestsWorker _interestsWorker;
        private readonly TripMessagesWorker _tripMessagesWorker;

        public TripController()
        {
            _tripsWorker = new TripsWorker();
            _userWorker = new UserWorker();
            _userInterestWorker = new UserInterestWorker();
            _tripsUsersWorker = new TripsUsersWorker();
            _interestsWorker = new InterestsWorker();
            _tripMessagesWorker = new TripMessagesWorker();
        }

        [HttpPost("[action]")]
        public IActionResult CreateTrip(int userId, [FromBody] TripCreateModel tripCreate)
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
        public IActionResult GetUserHeaderTrips(int userId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

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
        public IActionResult GetUserInterestForTrip(int userId, int tripId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

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
        public IActionResult IsUserAdmin(int userId, int tripId)
        {
            var result = _tripsUsersWorker.IsUserAdmin(userId, tripId);
            return Ok(result);
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
        public IActionResult AddNewTripMember(int adminId, int tripid, string newMemberEmail)
        {
            var result = _tripsWorker.AddNewTripMember(adminId, tripid, newMemberEmail);
            return Ok(result);
        }
    }
}