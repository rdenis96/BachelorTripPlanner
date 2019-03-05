using DataLayer.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Models
{
    public class UserInterest
    {
        public int UserId { get; set; }
        public string Discriminator { get; set; }
    }

    public class UserInterestCountryAndCity : UserInterest
    {
        public string Countries { get; set; }
        public string Cities { get; set; }
    }

    public class UserInterestWeather : UserInterest
    {
        public WeathersEnum Weathers { get; set; }
    }

    public class UserInterestTouristAttraction : UserInterest
    {
        public string TouristAttractions { get; set; }
    }

    public class UserInterestTransport : UserInterest
    {
        public TransportsEnum Transports { get; set; }
    }

    public class UserInterestExtended
    {
        public int UserId { get; set; }
        public string Countries { get; set; }
        public string Cities { get; set; }
        public WeathersEnum Weathers { get; set; }
        public string TouristAttractions { get; set; }
        public TransportsEnum Transports { get; set; }
    }
}