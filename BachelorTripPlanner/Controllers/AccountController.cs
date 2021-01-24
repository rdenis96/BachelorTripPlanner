using BachelorTripPlanner.Attributes;
using BachelorTripPlanner.Models;
using BusinessLogic.Accounts;
using BusinessLogic.Interests;
using BusinessLogic.Notifications;
using BusinessLogic.Trips;
using DataLayer.CompositionRoot;
using Domain.Common.Enums;
using Domain.Interests;
using Domain.Notifications;
using Domain.Notifications.Enums;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BachelorTripPlanner.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserWorker _userWorker;
        private readonly UserInterestWorker _userInterestWorker;
        private readonly InterestsWorker _interestsWorker;
        private readonly TripsUsersWorker _tripsUsersWorker;
        private readonly FriendsWorker _friendsWorker;
        private readonly NotificationsWorker _notificationsWorker;

        public AccountController(ICompositionRoot compositionRoot)
        {
            _userWorker = compositionRoot.GetImplementation<UserWorker>();
            _userInterestWorker = compositionRoot.GetImplementation<UserInterestWorker>();
            _interestsWorker = compositionRoot.GetImplementation<InterestsWorker>();
            _tripsUsersWorker = compositionRoot.GetImplementation<TripsUsersWorker>();
            _friendsWorker = compositionRoot.GetImplementation<FriendsWorker>();
            _notificationsWorker = compositionRoot.GetImplementation<NotificationsWorker>();
        }

        [HttpGet("[action]")]
        public IActionResult GetUser([FromQuery][ValidateUser] int userId)
        {
            var user = _userWorker.GetById(userId);
            return Ok(user);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableCountries([FromQuery][ValidateUser] int userId, [FromQuery] int? tripId = null)
        {
            var userCountries = _userInterestWorker.GetByUserIdAndTripId(userId, tripId)?.Countries?.ConvertStringToList(',');
            if (userCountries == null)
            {
                userCountries = new List<string>();
            }
            var countries = _interestsWorker.GetAllCountries();
            ExcludeListFromList(ref countries, userCountries);
            return Ok(countries);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableCities([FromQuery][ValidateUser] int userId, [FromQuery] List<string> availableCountries, [FromQuery] int? tripId = null)
        {
            var userCities = _userInterestWorker.GetByUserIdAndTripId(userId, tripId)?.Cities?.ConvertStringToList(',');
            if (userCities == null)
            {
                userCities = new List<string>();
            }
            var cities = _interestsWorker.GetAllCitiesByCountries(availableCountries);
            ExcludeListFromList(ref cities, userCities);
            return Ok(cities);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableWeather([FromQuery][ValidateUser] int userId, [FromQuery] int? tripId = null)
        {
            var userWeather = _userInterestWorker.GetByUserIdAndTripId(userId, tripId)?.Weather?.ConvertStringToList(',');
            if (userWeather == null)
            {
                userWeather = new List<string>();
            }
            var weather = _interestsWorker.GetAllWeather();
            ExcludeListFromList(ref weather, userWeather);

            return Ok(weather);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableTransport([FromQuery][ValidateUser] int userId, [FromQuery] int? tripId = null)
        {
            var userTransport = _userInterestWorker.GetByUserIdAndTripId(userId, tripId)?.Transports?.ConvertStringToList(',');
            if (userTransport == null)
            {
                userTransport = new List<string>();
            }
            var transports = _interestsWorker.GetAllTransports();
            ExcludeListFromList(ref transports, userTransport);

            return Ok(transports);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableTouristAttractions([FromQuery][ValidateUser] int userId, [FromQuery] int? tripId = null)
        {
            var userTouristAttractions = _userInterestWorker.GetByUserIdAndTripId(userId, tripId)?.TouristAttractions?.ConvertStringToList('#');
            if (userTouristAttractions == null)
            {
                userTouristAttractions = new List<string>();
            }
            var touristAttractions = _interestsWorker.GetAllTouristAttractions();
            ExcludeListFromList(ref touristAttractions, userTouristAttractions);

            return Ok(touristAttractions);
        }

        [HttpGet("[action]")]
        public IActionResult GetUserInterest([FromQuery][ValidateUser] int userId, [FromQuery] int? tripId = null)
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
        public IActionResult GetUserCitiesByUserCountriesAndAvailableCities([FromQuery][ValidateUser] int userId, [FromQuery] List<string> userCountries, [FromQuery] int? tripId = null)
        {
            var userCities = _userInterestWorker.GetByUserIdAndTripId(userId, tripId).Cities.ConvertStringToList(',');
            var allCitiesByCountries = _interestsWorker.GetAllCitiesByCountries(userCountries);
            var availableCities = allCitiesByCountries.ToList();
            ExcludeListFromList(ref availableCities, userCities);
            ExcludeListFromList(ref userCities, availableCities);
            userCities = userCities.Intersect(allCitiesByCountries).ToList();
            var result = new
            {
                availableCities,
                userCities
            };
            return Ok(result);
        }

        [HttpPut("[action]")]
        public IActionResult Update([ValidateUser] int userId, [FromBody] UserLoginModel userLoginModel)
        {
            var user = _userWorker.GetById(userId);

            user.Password = UserHelper.MD5Hash(userLoginModel.Password);

            user = _userWorker.Update(user);
            if (user == null)
            {
                return BadRequest("There was a problem updating the account, please try again later!");
            }

            return Ok();
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByCountryAndCity([ValidateUser] int userId, [FromBody] UserCountriesAndCitiesModel userCountriesAndCities)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userCountriesAndCities.TripId);
            userInterest.Countries = userCountriesAndCities.Countries.ConvertListToString(',');
            userInterest.Cities = userCountriesAndCities.Cities.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByWeather([ValidateUser] int userId, [FromBody] UserWeatherModel userWeather)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userWeather.TripId);
            userInterest.Weather = userWeather.Weather.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByTransport([ValidateUser] int userId, [FromBody] UserTransportModel userTransport)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userTransport.TripId);
            userInterest.Transports = userTransport.Transport.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByTouristAttractions([ValidateUser] int userId, [FromBody] UserTouristAttractionsModel userTouristAttractions)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userTouristAttractions.TripId);
            userInterest.TouristAttractions = userTouristAttractions.TouristAttractions.ConvertListToString('#');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpGet("[action]")]
        public IActionResult GetUserTrips([ValidateUser] int userId)
        {
            var trips = _tripsUsersWorker.GetTripsForUser(userId, includeDeleted: true);
            return Ok(trips);
        }

        [HttpGet("[action]")]
        public IActionResult GetFriends([ValidateUser] int userId)
        {
            var friends = _friendsWorker.GetByUserId(userId);
            return Ok(friends);
        }

        [HttpPost("[action]")]
        public IActionResult CreateFriend([FromBody] CreateFriendViewModel createFriendViewModel)
        {
            var friendAccount = _userWorker.GetByEmail(createFriendViewModel.FriendEmail);
            if (friendAccount == null)
            {
                return BadRequest("The friend account could not be found!");
            }

            var notificationExists = _notificationsWorker.GetByUserId(friendAccount.Id).Any(x => x.Type == NotificationType.FriendRequest && x.SenderId == createFriendViewModel.UserId);
            if (notificationExists)
            {
                return BadRequest("Friend request was already sent!");
            }

            var notification = _notificationsWorker.Create(new Notification
            {
                SenderId = createFriendViewModel.UserId,
                UserId = friendAccount.Id,
                Type = NotificationType.FriendRequest,
                Date = DateTime.Now
            });

            if (notification == null)
            {
                return BadRequest("The friend request couldn't be created. Please try again!");
            }
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult RemoveFriend(int id)
        {
            var isDeleted = _friendsWorker.Remove(id);
            if (isDeleted == false)
            {
                return BadRequest("The friend couldn't be deleted!");
            }

            return Ok();
        }

        private void ExcludeListFromList(ref List<string> sourceList, List<string> toExcludeList)
        {
            sourceList = sourceList.Except(toExcludeList).ToList();
        }

        private void ExcludeWeatherEnumFromEnum(ref WeatherEnum sourceEnum, WeatherEnum toExcludeEnum)
        {
            sourceEnum = sourceEnum & ~toExcludeEnum;
        }
    }
}