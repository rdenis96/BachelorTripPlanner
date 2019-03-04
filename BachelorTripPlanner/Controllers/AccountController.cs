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
        public IActionResult UpdateInterestByCountryAndCity(int userId, CountriesEnum country, string city)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }

            UserInterestCountryAndCity userInterestCountryAndCity = new UserInterestCountryAndCity();
            userInterestCountryAndCity.UserId = userId;
            userInterestCountryAndCity.Countries = country;
            userInterestCountryAndCity.Cities = city;

            var userInterest = _userInterestWorker.UpdateByCountryAndCity(userInterestCountryAndCity);
            return Ok(userInterest);
        }
    }
}