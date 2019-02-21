using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using BachelorTripPlanner.Models;
using BachelorTripPlanner.Workers;
using DataLayer.Helpers;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BachelorTripPlanner.Controllers
{
    [Route("api/[controller]")]
    public class LandingPageController : Controller
    {
        private UserWorker _userWorker;

        public LandingPageController()
        {
            _userWorker = new UserWorker();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody]UserRegisterModel userRegister)
        {
            var userExists = _userWorker.GetByEmail(userRegister.Email);
            if (userExists != null)
            {
                return BadRequest("User email exists, please login!");
            }
            var isEmailValid = UserHelper.IsEmailValid(userRegister.Email);
            if (isEmailValid == false)
            {
                return BadRequest("The email format is not valid!");
            }

            var user = new User()
            {
                Email = userRegister.Email,
                Password = UserHelper.MD5Hash(userRegister.Password),
                Ip = userRegister.Ip,
                Phone = userRegister.Phone
            };

            user = _userWorker.Create(user);
            if (user == null)
            {
                return BadRequest("The user could not be created, please try again later!");
            }

            var result = new
            {
                message = "The account was successfuly registered, please login!"
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login([FromQuery]UserLoginModel userLogin)
        {
            var user = _userWorker.GetByEmailAndPassword(userLogin.Email, userLogin.Password);
            if (user == null)
            {
                return BadRequest("User does not exist or the credentials are wrong, please register or try again!");
            }

            var result = new
            {
                userId = user.Id,
                message = "Login successful, you will be redirected!"
            };
            return Ok(result);
        }
    }
}