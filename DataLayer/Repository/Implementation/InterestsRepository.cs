using DataLayer.Context;
using DataLayer.Helpers;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repository.Implementation
{
    public class InterestsRepository : IInterestsRepository
    {
        private IUserInterestRepository _userInterestsRepository;

        public InterestsRepository()
        {
            _userInterestsRepository = new UserInterestRepository();
        }

        public Interest Create(Interest obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Interest obj)
        {
            throw new NotImplementedException();
        }

        public ICollection<Interest> GetAll()
        {
            using (TripPlanner context = new TripPlanner())
            {
                var interests = context.Interests.ToList();

                if (interests != null)
                {
                    return interests;
                }
                return null;
            }
        }

        public List<string> GetAllCitiesByCountries(List<string> countries)
        {
            if (countries == null)
                return null;

            using (TripPlanner context = new TripPlanner())
            {
                var interests = context.Interests.Where(x => countries.Contains(x.Country)).Select(x => x.City).ToList();

                if (interests != null)
                {
                    return interests;
                }
                return null;
            }
        }

        public List<string> GetAllCitiesByCountry(string country)
        {
            if (country == null)
                return null;

            using (TripPlanner context = new TripPlanner())
            {
                var interests = context.Interests.Where(x => x.Country == country).Select(x => x.City).ToList();

                if (interests != null)
                {
                    return interests;
                }
                return null;
            }
        }

        public List<string> GetAllCountries()
        {
            using (TripPlanner context = new TripPlanner())
            {
                var interests = context.Interests.Select(x => x.Country).Distinct().ToList();

                if (interests != null)
                {
                    return interests;
                }
                return null;
            }
        }

        public Interest GetById(int id)
        {
            if (id < 0)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var interest = context.Interests.Find(id);

                if (interest != null)
                {
                    return interest;
                }
                return null;
            }
        }

        public List<Interest> GetByCountries(List<string> countries)
        {
            if (countries == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var interests = context.Interests.Where(x => countries.Contains(x.Country)).ToList();

                if (interests != null)
                {
                    return interests;
                }
                return null;
            }
        }

        public List<Interest> GetByCountry(string country)
        {
            if (country == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var interests = context.Interests.Where(x => x.Country == country).ToList();

                if (interests != null)
                {
                    return interests;
                }
                return null;
            }
        }

        public Interest Update(Interest obj)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllWeather()
        {
            using (TripPlanner context = new TripPlanner())
            {
                var weather = context.Interests.Select(x => x.Weather).ToList();

                if (weather != null)
                {
                    List<string> weatherDistinct = new List<string>();
                    foreach (var item in weather)
                    {
                        var splitString = item.ConvertStringToList(',').Select(x => x.Trim()).ToList();
                        weatherDistinct = weatherDistinct.Union(splitString).ToList();
                    }

                    return weatherDistinct;
                }

                return null;
            }
        }

        public List<string> GetAllTransports()
        {
            using (TripPlanner context = new TripPlanner())
            {
                var transports = context.Interests.Select(x => x.Transport).ToList();

                if (transports != null)
                {
                    List<string> transportDistinct = new List<string>();
                    foreach (var item in transports)
                    {
                        var splitString = item.ConvertStringToList(',').Select(x => x.Trim()).ToList();
                        transportDistinct = transportDistinct.Union(splitString).ToList();
                    }

                    return transportDistinct;
                }

                return null;
            }
        }

        public List<string> GetAllTouristAttractions()
        {
            using (TripPlanner context = new TripPlanner())
            {
                var touristAttractions = context.Interests.Select(x => x.TouristAttractions).ToList();

                if (touristAttractions != null)
                {
                    List<string> touristAttractionsDistinct = new List<string>();
                    foreach (var item in touristAttractions)
                    {
                        var splitString = item.ConvertStringToList('#').Where(x => string.IsNullOrWhiteSpace(x) == false).Select(x => x.Trim()).ToList();
                        touristAttractionsDistinct = touristAttractionsDistinct.Union(splitString).ToList();
                    }

                    return touristAttractionsDistinct;
                }

                return null;
            }
        }

        public List<Interest> GetSuggestedInterests(int userId)
        {
            using (TripPlanner context = new TripPlanner())
            {
                var userInterests = _userInterestsRepository.GetByUserId(userId);

                if (userInterests == null)
                {
                    return new List<Interest>();
                }

                var userInterestsConverted = new
                {
                    Countries = userInterests.Countries.ConvertStringToList(','),
                    Cities = userInterests.Cities.ConvertStringToList(','),
                    Weather = userInterests.Weather.ConvertStringToList(','),
                    TouristAttractions = userInterests.TouristAttractions.ConvertStringToList('#'),
                    Transports = userInterests.Transports.ConvertStringToList(',')
                };

                List<Interest> interestsByCountries = context.Interests.Where(x => userInterestsConverted.Countries.Contains(x.Country)).ToList();
                List<Interest> interestsByCities = context.Interests.Where(x => userInterestsConverted.Cities.Contains(x.City)).ToList();
                List<Interest> interestsByWeather = context.Interests.Where(x => userInterestsConverted.Weather.Any(val => x.Weather.Contains(val))).ToList();
                List<Interest> interestsByTouristAttractions = context.Interests.Where(x => userInterestsConverted.TouristAttractions.Any(val => x.TouristAttractions.Contains(val))).ToList();
                List<Interest> interestsByTransports = context.Interests.Where(x => userInterestsConverted.Transports.Any(val => x.Transport.Contains(val))).ToList();
                var interestsUnion = interestsByCountries.Union(interestsByCities).Union(interestsByWeather).Union(interestsByTouristAttractions).Union(interestsByTransports).ToList().Shuffle().Take(20).ToList();
                return interestsUnion;
            }
        }

        public List<Interest> GetSuggestedInterestsByTrip(int tripId)
        {
            using (TripPlanner context = new TripPlanner())
            {
                var allTripUsersInterests = context.UserInterests.Where(x => x.TripId == tripId);

                var userInterestsConverted = new
                {
                    Countries = new List<string>(),
                    Cities = new List<string>(),
                    Weather = new List<string>(),
                    TouristAttractions = new List<string>(),
                    Transports = new List<string>()
                };

                foreach (var userInterests in allTripUsersInterests)
                {
                    userInterestsConverted.Countries.AddRange(userInterests.Countries.ConvertStringToList(','));
                    userInterestsConverted.Cities.AddRange(userInterests.Cities.ConvertStringToList(','));
                    userInterestsConverted.Weather.AddRange(userInterests.Weather.ConvertStringToList(','));
                    userInterestsConverted.TouristAttractions.AddRange(userInterests.TouristAttractions.ConvertStringToList('#'));
                    userInterestsConverted.Transports.AddRange(userInterests.Transports.ConvertStringToList(','));
                }

                List<Interest> interestsByCountries = context.Interests.Where(x => userInterestsConverted.Countries.Contains(x.Country)).ToList();
                List<Interest> interestsByCities = context.Interests.Where(x => userInterestsConverted.Cities.Contains(x.City)).ToList();
                List<Interest> interestsByWeather = context.Interests.Where(x => userInterestsConverted.Weather.Any(val => x.Weather.Contains(val))).ToList();
                List<Interest> interestsByTouristAttractions = context.Interests.Where(x => userInterestsConverted.TouristAttractions.Any(val => x.TouristAttractions.Contains(val))).ToList();
                List<Interest> interestsByTransports = context.Interests.Where(x => userInterestsConverted.Transports.Any(val => x.Transport.Contains(val))).ToList();
                var interestsUnion = interestsByCountries.Union(interestsByCities).Union(interestsByWeather).Union(interestsByTouristAttractions).Union(interestsByTransports).ToList().Shuffle().ToList();
                return interestsUnion;
            }
        }

        public List<Interest> GetRandomInterests()
        {
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.Interests.ToList().Shuffle().Take(20).ToList();
                return result;
            }
        }
    }
}