using Domain.Trips.Enums;
using System.Collections.Generic;

namespace BachelorTripPlanner.Models
{
    public class TripCreateModel
    {
        public string TripName { get; set; }
        public TripType TripType { get; set; }
        public List<string> InvitedPeople { get; set; }
    }
}