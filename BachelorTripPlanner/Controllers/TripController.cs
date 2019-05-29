using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorTripPlanner.Models;
using BachelorTripPlanner.Workers;
using DataLayer.Enums;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BachelorTripPlanner.Controllers
{
    [Route("api/[controller]")]
    public class TripController : Controller
    {
        private TripsWorker _tripsWorker;

        public TripController()
        {
            _tripsWorker = new TripsWorker();
        }

        [HttpPost("[action]")]
        public IActionResult CreateTrip(int userId, [FromBody]TripCreateModel tripCreate)
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
    }
}