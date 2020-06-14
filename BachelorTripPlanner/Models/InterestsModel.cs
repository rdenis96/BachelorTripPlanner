using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Models
{
    public class InterestsModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public List<string> Weather { get; set; }
        public List<string> TouristAttractions { get; set; }
        public List<string> Transports { get; set; }
        public string LinkImage { get; set; }
        public string LinkWikipediaCity { get; set; }
    }
}