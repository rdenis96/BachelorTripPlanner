using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Context;
using DataLayer.Models;

namespace DataLayer.Repository.Implementation
{
    public class UserInterestRepository : IUserInterestRepository
    {
        public UserInterest Create(UserInterest obj)
        {
            using (TripPlanner context = new TripPlanner())
            {
                context.UserInterests.Add(obj);
                context.SaveChanges();
                return obj;
            }
        }

        public bool Delete(UserInterest obj)
        {
            if (obj == null)
                return false;
            using (TripPlanner context = new TripPlanner())
            {
                context.UserInterests.Remove(obj);
                if (context.UserInterests.Contains(obj))
                    return false;
                context.SaveChanges();
                return true;
            }
        }

        public ICollection<UserInterest> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserInterest GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserInterestExtended GetByUserId(int userId)
        {
            if (userId <= 0)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                var result = context.UserInterests.Where(x => x.UserId == userId).FirstOrDefault();
                if (result != null)
                {
                    UserInterestExtended userInterest = new UserInterestExtended();
                    userInterest.UserId = result.UserId;
                    userInterest.Countries = ((UserInterestCountryAndCity)result).Countries;
                    userInterest.Cities = ((UserInterestCountryAndCity)result).Cities;
                    userInterest.Transports = ((UserInterestTransport)result).Transports;
                    userInterest.TouristAttractions = ((UserInterestTouristAttraction)result).TouristAttractions;
                    userInterest.Weathers = ((UserInterestWeather)result).Weathers;
                    return userInterest;
                }
                return null;
            }
        }

        public UserInterest Update(UserInterest obj)
        {
            throw new NotImplementedException();
        }

        public UserInterest UpdateByCountryAndCity(UserInterestCountryAndCity userInterestCountryAndCity)
        {
            if (userInterestCountryAndCity == null)
                return null;
            using (TripPlanner context = new TripPlanner())
            {
                context.Entry(userInterestCountryAndCity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.SaveChanges();

                var result = context.UserInterests.Find(userInterestCountryAndCity.UserId);
                return result;
            }
        }

        public UserInterest UpdateByTouristAttraction(UserInterestTouristAttraction userInterestTouristAttraction)
        {
            throw new NotImplementedException();
        }

        public UserInterest UpdateByTransport(UserInterestTransport userInterestTransport)
        {
            throw new NotImplementedException();
        }

        public UserInterest UpdateByWeather(UserInterestWeather userInterestWeather)
        {
            throw new NotImplementedException();
        }
    }
}