using System.Collections.Generic;

namespace BachelorTripPlanner.Models
{
    public class UserTouristAttractionsModel
    {
        public List<string> TouristAttractions { get; set; }
        public int? TripId { get; set; }
    }
}