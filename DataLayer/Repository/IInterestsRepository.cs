using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

        List<Interest> GetRandomInterests();
    }
}