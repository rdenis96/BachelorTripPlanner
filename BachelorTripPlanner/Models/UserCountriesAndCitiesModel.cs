using System.Collections.Generic;

namespace BachelorTripPlanner.Models
{
    public class UserCountriesAndCitiesModel
    {
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }
        public int? TripId { get; set; }
    }
}