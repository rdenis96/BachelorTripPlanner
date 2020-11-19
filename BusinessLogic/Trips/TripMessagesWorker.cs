using Domain.Repository;
using Domain.Trips;
using System.Collections.Generic;

namespace BusinessLogic.Trips
{
    public class TripMessagesWorker
    {
        private readonly ITripMessagesRepository _tripMessagesRepository;

        public TripMessagesWorker(ITripMessagesRepository tripMessagesRepository)
        {
            _tripMessagesRepository = tripMessagesRepository;
        }

        public TripMessage Create(TripMessage obj)
        {
            var result = _tripMessagesRepository.Create(obj);
            return result;
        }

        public bool Delete(TripMessage obj)
        {
            var result = _tripMessagesRepository.Delete(obj);
            return result;
        }

        public ICollection<TripMessage> GetAll()
        {
            var result = _tripMessagesRepository.GetAll();
            return result;
        }

        public IEnumerable<TripMessage> GetByTripId(int tripid)
        {
            var result = _tripMessagesRepository.GetByTripId(tripid);
            return result;
        }

        public TripMessage GetById(int id)
        {
            var result = _tripMessagesRepository.GetById(id);
            return result;
        }

        public TripMessage Update(TripMessage obj)
        {
            var result = _tripMessagesRepository.Update(obj);
            return result;
        }
    }
}