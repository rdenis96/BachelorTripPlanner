using BachelorTripPlanner.Models;
using BachelorTripPlanner.Workers;
using DataLayer.Enums;
using DataLayer.Helpers;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private UserWorker _userWorker;
        private UserInterestWorker _userInterestWorker;
        private InterestsWorker _interestsWorker;

        public AccountController()
        {
            _userWorker = new UserWorker();
            _userInterestWorker = new UserInterestWorker();
            _interestsWorker = new InterestsWorker();
        }

        [HttpGet("[action]")]
        public IActionResult GetUser([FromQuery]int userId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            return Ok(user);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableCountries([FromQuery]int userId)
        {
            var userCountries = _userInterestWorker.GetByUserId(userId)?.Countries.ConvertStringToList(',');
            var countries = _interestsWorker.GetAllCountries();
            ExcludeListFromList(ref countries, userCountries);
            return Ok(countries);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableCities([FromQuery]int userId, [FromQuery]List<string> availableCountries)
        {
            var userCities = _userInterestWorker.GetByUserId(userId)?.Cities.ConvertStringToList(',');
            var cities = _interestsWorker.GetAllCitiesByCountries(availableCountries);
            ExcludeListFromList(ref cities, userCities);
            return Ok(cities);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableWeather([FromQuery]int userId)
        {
            var userWeather = _userInterestWorker.GetByUserId(userId)?.Weather.ConvertStringToList(',');
            var weather = _interestsWorker.GetAllWeather();
            ExcludeListFromList(ref weather, userWeather);

            return Ok(weather);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableTransport([FromQuery]int userId)
        {
            var userTransport = _userInterestWorker.GetByUserId(userId)?.Transports.ConvertStringToList(',');
            var transports = _interestsWorker.GetAllTransports();
            ExcludeListFromList(ref transports, userTransport);

            return Ok(transports);
        }

        [HttpGet("[action]")]
        public IActionResult GetAvailableTouristAttractions([FromQuery]int userId)
        {
            var userTouristAttractions = _userInterestWorker.GetByUserId(userId)?.TouristAttractions.ConvertStringToList('#');
            var touristAttractions = _interestsWorker.GetAllTouristAttractions();
            ExcludeListFromList(ref touristAttractions, userTouristAttractions);

            return Ok(touristAttractions);
        }

        [HttpGet("[action]")]
        public IActionResult GetUserInterest([FromQuery]int userId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            var userInterest = _userInterestWorker.GetByUserId(userId);
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
        public IActionResult GetUserCitiesByUserCountriesAndAvailableCities([FromQuery]int userId, [FromQuery]List<string> userCountries)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            var userCities = _userInterestWorker.GetByUserId(userId).Cities.ConvertStringToList(',');
            var allCitiesByCountries = _interestsWorker.GetAllCitiesByCountries(userCountries);
            var availableCities = allCitiesByCountries.ToList();
            ExcludeListFromList(ref availableCities, userCities);
            ExcludeListFromList(ref userCities, availableCities);
            userCities = userCities.Intersect(allCitiesByCountries).ToList();
            var result = new
            {
                availableCities = availableCities,
                userCities = userCities
            };
            return Ok(result);
        }

        [HttpPut("[action]")]
        public IActionResult Update(int userId, [FromBody]UserLoginModel userLoginModel)
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
        public IActionResult UpdateInterestByCountryAndCity(int userId, [FromBody]UserCountriesAndCitiesModel userCountriesAndCities)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            UserInterest userInterest = _userInterestWorker.GetByUserId(userId);
            userInterest.Countries = userCountriesAndCities.Countries.ConvertListToString(',');
            userInterest.Cities = userCountriesAndCities.Cities.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByWeather(int userId, [FromBody]UserWeatherModel userWeather)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            UserInterest userInterest = _userInterestWorker.GetByUserId(userId);
            userInterest.Weather = userWeather.Weather.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByTransport(int userId, [FromBody]UserTransportModel userTransport)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            UserInterest userInterest = _userInterestWorker.GetByUserId(userId);
            userInterest.Transports = userTransport.Transport.ConvertListToString(',');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateInterestByTouristAttractions(int userId, [FromBody]UserTouristAttractionsModel userTouristAttractions)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            UserInterest userInterest = _userInterestWorker.GetByUserId(userId);
            userInterest.TouristAttractions = userTouristAttractions.TouristAttractions.ConvertListToString('#');

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
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