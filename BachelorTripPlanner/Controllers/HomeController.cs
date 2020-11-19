using BachelorTripPlanner.Models;
using BusinessLogic.Accounts;
using BusinessLogic.Interests;
using DataLayer.CompositionRoot;
using Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BachelorTripPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserWorker _userWorker;
        private readonly InterestsWorker _interestWorker;

        public HomeController(ICompositionRoot compositionRoot)
        {
            _userWorker = compositionRoot.GetImplementation<UserWorker>();
            _interestWorker = compositionRoot.GetImplementation<InterestsWorker>();
        }

        [HttpGet("{*url}")]
        public IActionResult Index()
        {
            var currentRequest = Request.GetDisplayUrl();
            if (currentRequest.Contains("/api/"))
            {
                return NotFound();
            }
            return View();
        }

        [HttpGet]
        [Route("api/home/[action]")]
        public IActionResult GetSuggestedInterests(int userId)
        {
            var user = _userWorker.GetById(userId);
            if (user == null)
            {
                return BadRequest("The account could not be retrieved!");
            }
            var randomInterests = _interestWorker.GetSuggestedInterests(userId);

            var result = new List<InterestsModel>();
            foreach (var interest in randomInterests)
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

        [HttpGet]
        [Route("api/home/[action]")]
        public IActionResult GetRandomInterests()
        {
            var randomInterests = _interestWorker.GetRandomInterests();

            var result = new List<InterestsModel>();
            foreach (var interest in randomInterests)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}