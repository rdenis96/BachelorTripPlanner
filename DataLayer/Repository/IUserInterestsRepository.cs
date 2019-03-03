using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repository
{
    public interface IUserInterestsRepository : IBasicRepository<UserInterest>
    {
        UserInterest GetByUserId(int userId);

        UserInterest UpdateByCountryAndCity(UserInterestCountryAndCity userInterestCountryAndCity);

        UserInterest UpdateByWeather(UserInterestWeather userInterestWeather);

        UserInterest UpdateByTouristAttraction(UserInterestTouristAttraction userInterestTouristAttraction);

        UserInterest UpdateByTransport(UserInterestTransport userInterestTransport);
    }
}