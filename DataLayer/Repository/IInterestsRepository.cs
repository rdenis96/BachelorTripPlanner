using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Repository
{
    public interface IInterestsRepository : IBasicRepository<Interest>
    {
        List<Interest> GetByCountry(string country);

        List<Interest> GetByCountries(List<string> countries);

        List<string> GetAllCountries();

        List<string> GetAllCitiesByCountry(string country);

        List<string> GetAllCitiesByCountries(List<string> countries);

        List<string> GetAllWeather();

        List<string> GetAllTransports();

        List<string> GetAllTouristAttractions();

        List<Interest> GetSuggestedInterests(int userId);

        List<Interest> GetSuggestedInterestsByTrip(int tripId);

        List<Interest> GetRandomInterests();
    }
}