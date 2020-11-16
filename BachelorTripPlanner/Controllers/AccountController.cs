using BachelorTripPlanner.Models;
using BachelorTripPlanner.Workers;
using DataLayer.Enums;
using DataLayer.Helpers;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BachelorTripPlanner.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private UserWorker _userWorker;
        private UserInterestWorker _userInterestWorker;
        private InterestsWorker _interestsWorker;
        private TripsUsersWorker _tripsUsersWorker;

        public AccountController()
        {
            _userWorker = new UserWorker();
            _userInterestWorker = new UserInterestWorker();
            _interestsWorker = new InterestsWorker();
            _tripsUsersWorker = new TripsUsersWorker();
        }

        [HttpGet("[action]")]
        public IActionResult GetUser([FromQuery] int userId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            return Ok(user);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableCountries([FromQuery] int userId, [FromQuery] int? tripId = null)
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
        public IActionResult GetAvailableCities([FromQuery] int userId, [FromQuery] List<string> availableCountries, [FromQuery] int? tripId = null)
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
        public IActionResult GetAvailableWeather([FromQuery] int userId, [FromQuery] int? tripId = null)
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
        public IActionResult GetAvailableTransport([FromQuery] int userId, [FromQuery] int? tripId = null)
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
        public IActionResult GetAvailableTouristAttractions([FromQuery] int userId, [FromQuery] int? tripId = null)
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
        public IActionResult GetUserInterest([FromQuery] int userId, [FromQuery] int? tripId = null)
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
        public IActionResult GetUserCitiesByUserCountriesAndAvailableCities([FromQuery] int userId, [FromQuery] List<string> userCountries, [FromQuery] int? tripId = null)
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
        public IActionResult Update(int userId, [FromBody] UserLoginModel userLoginModel)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            user.Password = UserHelper.MD5Hash(userLoginModel.Password);

            user = _userWorker.Update(user);
            if (user == null)
            {
                return BadRequest("There was a problem updating the account, please try again later!");
            }

            return Ok();
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByCountryAndCity(int userId, [FromBody] UserCountriesAndCitiesModel userCountriesAndCities)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userCountriesAndCities.TripId);
            userInterest.Countries = userCountriesAndCities.Countries.ConvertListToString(',');
            userInterest.Cities = userCountriesAndCities.Cities.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByWeather(int userId, [FromBody] UserWeatherModel userWeather)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userWeather.TripId);
            userInterest.Weather = userWeather.Weather.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByTransport(int userId, [FromBody] UserTransportModel userTransport)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userTransport.TripId);
            userInterest.Transports = userTransport.Transport.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByTouristAttractions(int userId, [FromBody] UserTouristAttractionsModel userTouristAttractions)
        {
            UserInterest userInterest = _userInterestWorker.GetByUserIdAndTripId(userId, userTouristAttractions.TripId);
            userInterest.TouristAttractions = userTouristAttractions.TouristAttractions.ConvertListToString('#');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpGet("[action]")]
        public IActionResult GetUserTrips(int userId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            var trips = _tripsUsersWorker.GetTripsForUser(userId, includeDeleted: true);
            return Ok(trips);
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