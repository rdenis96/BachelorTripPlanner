using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BachelorTripPlanner.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace BachelorTripPlanner.Controllers
{
    public class HomeController : Controller
    {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}