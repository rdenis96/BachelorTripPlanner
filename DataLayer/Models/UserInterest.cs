using DataLayer.Enums;

namespace DataLayer.Models
{
    public class UserInterest
    {
        public int UserId { get; set; }
    }

    public class UserInterestCountryAndCity : UserInterest
    {
        public CountriesEnum Countries { get; set; }
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

    public class UserInterestExtended : UserInterest
    {
        public CountriesEnum Countries { get; set; }
        public string Cities { get; set; }
        public WeathersEnum Weathers { get; set; }
        public string TouristAttractions { get; set; }
        public TransportsEnum Transports { get; set; }
    }
}