using DataLayer.Models;
using DataLayer.Repository;
using DataLayer.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Workers
{
    public class UserInterestWorker
    {
        private IUserInterestRepository _userInterestRepository;

        public UserInterestWorker()
        {
            _userInterestRepository = new UserInterestRepository();
        }

        public UserInterest UpdateByCountryAndCity(UserInterestCountryAndCity userInterestCountryAndCity)
        {
            var result = _userInterestRepository.UpdateByCountryAndCity(userInterestCountryAndCity);
            return result;
        }
    }
}