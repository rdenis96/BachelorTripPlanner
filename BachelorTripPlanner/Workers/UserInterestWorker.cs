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

        public UserInterest GetByUserId(int userId)
        {
            var result = _userInterestRepository.GetByUserId(userId);
            return result;
        }

        public UserInterest Update(UserInterest userInterest)
        {
            var result = _userInterestRepository.Update(userInterest);
            return result;
        }
    }
}