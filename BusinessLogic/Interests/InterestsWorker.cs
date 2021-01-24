﻿using Domain.Interests;
using Domain.Repository;
using System.Collections.Generic;

namespace BusinessLogic.Interests
{
    public class InterestsWorker
    {
        private IInterestsRepository _interestsRepository;

        public InterestsWorker(IInterestsRepository interestsRepository)
        {
            _interestsRepository = interestsRepository;
        }

        public ICollection<Interest> GetAll()
        {
            var result = _interestsRepository.GetAll();
            return result;
        }

        public List<Interest> GetByCountry(string country)
        {
            var result = _interestsRepository.GetByCountry(country);
            return result;
        }

        public List<Interest> GetByCountries(List<string> countries)
        {
            var result = _interestsRepository.GetByCountries(countries);
            return result;
        }

        public List<string> GetAllCountries()
        {
            var result = _interestsRepository.GetAllCountries();
            return result;
        }

        public List<string> GetAllCitiesByCountry(string country)
        {
            var result = _interestsRepository.GetAllCitiesByCountry(country);
            return result;
        }

        public List<string> GetAllCitiesByCountries(List<string> countries)
        {
            var result = _interestsRepository.GetAllCitiesByCountries(countries);
            return result;
        }

        public List<string> GetAllWeather()
        {
            var result = _interestsRepository.GetAllWeather();
            return result;
        }

        public List<string> GetAllTransports()
        {
            var result = _interestsRepository.GetAllTransports();
            return result;
        }

        public List<string> GetAllTouristAttractions()
        {
            var result = _interestsRepository.GetAllTouristAttractions();
            return result;
        }

        public List<Interest> GetSuggestedInterests(int userId)
        {
            var result = _interestsRepository.GetSuggestedInterests(userId);
            return result;
        }

        public List<Interest> GetSuggestedInterestsByTrip(int tripId, int suggestedInterestsLevel = 0, bool isLoadMoreLevelPressed = false)
        {
            var result = _interestsRepository.GetSuggestedInterestsByTrip(tripId, suggestedInterestsLevel, isLoadMoreLevelPressed);
            return result;
        }

        public List<Interest> GetRandomInterests()
        {
            var result = _interestsRepository.GetRandomInterests();
            return result;
        }
    }
}