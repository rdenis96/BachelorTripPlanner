using Domain.Interests;
using System.Collections.Generic;

namespace Domain.Repository
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

        List<Interest> GetSuggestedInterestsByTrip(int tripId, int suggestedInterestsLevel = 0, bool isLoadMoreLevelPressed = false);

        List<Interest> GetRandomInterests();
    }
}