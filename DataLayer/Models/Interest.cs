using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DataLayer.Helpers;

namespace DataLayer.Models
{
    public class Interest : IEquatable<Interest>
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string GeneralWeather { get; set; }
        public string Weather { get; set; }
        public string TouristAttractions { get; set; }
        public string Transport { get; set; }
        public string LinkImage { get; set; }
        public string LinkWikipediaCity { get; set; }

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

            var result = Id == obj.Id &&
                         Country == obj.Country &&
                         City == obj.City &&
                         GeneralWeather == obj.GeneralWeather &&
                         Weather == obj.Weather &&
                         TouristAttractions == obj.TouristAttractions &&
                         Transport == obj.Transport &&
                         LinkImage == obj.LinkImage &&
                         LinkWikipediaCity == obj.LinkWikipediaCity;
            return result;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Country, City, GeneralWeather, Weather, TouristAttractions, Transport);
        }
    }
}