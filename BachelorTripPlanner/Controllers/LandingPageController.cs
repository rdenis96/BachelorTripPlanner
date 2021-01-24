using BachelorTripPlanner.Models;
using BusinessLogic.Accounts;
using BusinessLogic.Interests;
using DataLayer.CompositionRoot;
using Domain.Accounts;
using Domain.Common.Constants;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BachelorTripPlanner.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LandingPageController : Controller
    {
        private readonly UserWorker _userWorker;
        private readonly UserInterestWorker _userInterestWorker;

        public LandingPageController(ICompositionRoot compositionRoot)
        {
            _userWorker = compositionRoot.GetImplementation<UserWorker>();
            _userInterestWorker = compositionRoot.GetImplementation<UserInterestWorker>();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody] UserRegisterModel userRegister)
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

            _userInterestWorker.Create(user.Id, null);

            var result = new
            {
                message = "The account was successfuly registered, please login!"
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login([FromQuery] UserLoginModel userLogin)
        {
            var user = _userWorker.GetByEmailAndPassword(userLogin.Email, userLogin.Password);
            if (user == null)
            {
                return BadRequest("User does not exist or the credentials are wrong, please register or try again!");
            }

            var token = GenerateJwtToken(user);

            var result = new
            {
                userId = user.Id,
                token = token,
                message = "Login successful, you will be redirected!"
            };
            return Ok(result);
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GlobalConstants.JwtSecretKey));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

                var jwtSecurityToken = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: DateTime.UtcNow.AddMonths(3)
                    );

                var token = tokenHandler.WriteToken(jwtSecurityToken);
                return token;
            }
            catch
            {
                return null;
            }
        }
    }
}