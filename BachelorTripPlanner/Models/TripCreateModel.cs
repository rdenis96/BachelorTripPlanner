using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Models
{
    public class TripCreateModel
    {
        public string TripName { get; set; }
        public TripType TripType { get; set; }
        public List<string> InvitedPeople { get; set; }
    }
}