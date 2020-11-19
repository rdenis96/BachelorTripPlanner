using Domain.Common;
using System;

namespace Domain.Interests
{
    public class Interest : BaseEntity, IEquatable<Interest>
    {
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
            return Equals(obj as Interest);
        }

        public bool Equals(Interest other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   Country == other.Country &&
                   City == other.City &&
                   GeneralWeather == other.GeneralWeather &&
                   Weather == other.Weather &&
                   TouristAttractions == other.TouristAttractions &&
                   Transport == other.Transport &&
                   LinkImage == other.LinkImage &&
                   LinkWikipediaCity == other.LinkWikipediaCity;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Id);
            hash.Add(Country);
            hash.Add(City);
            hash.Add(GeneralWeather);
            hash.Add(Weather);
            hash.Add(TouristAttractions);
            hash.Add(Transport);
            hash.Add(LinkImage);
            hash.Add(LinkWikipediaCity);
            return hash.ToHashCode();
        }
    }
}