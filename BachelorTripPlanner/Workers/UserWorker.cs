using DataLayer.Models;
using DataLayer.Repository;
using DataLayer.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorTripPlanner.Workers
{
    public class UserWorker
    {
        private IUserRepository userRepository;

        public UserWorker()
        {
            userRepository = new UserRepository();
        }

        public User Create(User obj)
        {
            return userRepository.Create(obj);
        }

        public ICollection<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public User GetByEmail(string email)
        {
            return userRepository.GetByEmail(email);
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            return userRepository.GetByEmailAndPassword(email, password);
        }

        public User GetById(int id)
        {
            return userRepository.GetById(id);
        }

        public bool Delete(User obj)
        {
            return userRepository.Delete(obj);
        }

        public User Update(User obj)
        {
            return userRepository.Update(obj);
        }
    }
}