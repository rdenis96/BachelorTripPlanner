using System.Collections.Generic;

namespace BachelorTripPlanner.Models
{
    public class UserWeatherModel
    {
        public List<string> Weather { get; set; }
        public int? TripId { get; set; }
    }
}