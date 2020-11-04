using System.Collections.Generic;

namespace BachelorTripPlanner.Models
{
    public class UserTransportModel
    {
        public List<string> Transport { get; set; }
        public int? TripId { get; set; }
    }
}