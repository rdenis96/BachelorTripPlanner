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

        public AccountController()
        {
            _userWorker = new UserWorker();
            _userInterestWorker = new UserInterestWorker();
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
            var countriesEnumList = Enum.GetValues(typeof(CountriesEnum));
            var userCountries = _userInterestWorker.GetByUserId(userId)?.Countries?.ConvertCountriesStringToCountriesEnumList();
            List<string> countries = new List<string>();
            foreach (CountriesEnum countryEnum in countriesEnumList)
            {
                if (userCountries.Contains(countryEnum) == false)
                {
                    countries.Add(countryEnum.ToString());
                }
            }
            return Ok(countries);
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

            return Ok(userInterest);
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
        public IActionResult UpdateInterestByCountryAndCity(int userId, List<CountriesEnum> countries, string cities)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            UserInterest userInterest = _userInterestWorker.GetByUserId(userId);
            userInterest.Countries = countries.ConvertCountriesEnumListToString();
            userInterest.Cities = cities;

            userInterest = _userInterestWorker.Update(userInterest);
            return Ok(userInterest);
        }
    }
}