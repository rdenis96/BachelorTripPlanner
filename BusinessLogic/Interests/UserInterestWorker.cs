using Domain.Interests;
using Domain.Repository;

namespace BusinessLogic.Interests
{
    public class UserInterestWorker
    {
        private IUserInterestRepository _userInterestRepository;

        public UserInterestWorker(IUserInterestRepository userInterestRepository)
        {
            _userInterestRepository = userInterestRepository;
        }

        public UserInterest Create(int userId, int? tripId)
        {
            var userInterest = new UserInterest
            {
                UserId = userId,
                TripId = tripId,
                Cities = string.Empty,
                Countries = string.Empty,
                TouristAttractions = string.Empty,
                Transports = string.Empty,
                Weather = string.Empty
            };
            var result = _userInterestRepository.Create(userInterest);
            return result;
        }

        public UserInterest GetByUserId(int userId)
        {
            var result = _userInterestRepository.GetByUserId(userId);
            return result;
        }

        public UserInterest GetByUserIdAndTripId(int userId, int? tripId)
        {
            UserInterest result = null;
            if (tripId.HasValue == false || tripId == 0)
            {
                result = _userInterestRepository.GetByUserId(userId);
            }
            else
            {
                result = _userInterestRepository.GetByUserIdAndTripId(userId, tripId.Value);
            }
            return result;
        }

        public UserInterest Update(UserInterest userInterest)
        {
            var result = _userInterestRepository.Update(userInterest);
            return result;
        }
    }
}