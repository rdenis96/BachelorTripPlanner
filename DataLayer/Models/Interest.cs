using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Interest : IEquatable<Interest>
    {
        private List<string> _touristAttractionsList { get; set; }

        public CountriesEnum Country { get; set; }
        public string City { get; set; }
        public WeathersEnum GeneralWeather { get; set; }
        public WeathersEnum Weather { get; set; }
        public string TouristAttractions { get; set; }

        public List<string> TouristAttractionsList
        {
            get
            {
                var splitedString = TouristAttractions.Split(new Char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                _touristAttractionsList = new List<string>(splitedString);
                return _touristAttractionsList;
            }
        }

        public TransportsEnum Transport { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((Interest)obj);
        }

        public bool Equals(Interest obj)
        {
            if (obj == null)
                return false;

            var result = Country == obj.Country &&
                         City == obj.City &&
                         GeneralWeather == obj.GeneralWeather &&
                         Weather == obj.Weather &&
                         TouristAttractions == obj.TouristAttractions &&
                         Transport == obj.Transport;
            return result;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Country, City);
        }
    }
}